using Prime.Memory.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Utils;

namespace Prime.Memory
{
    class Dolphin
    {
        private static Process dolphin = null;
        private static bool Is32BitProcess = false;
        private static long RAMBaseAddr = 0;
        private static IntPtr GameWindowHandle = IntPtr.Zero;
        private static _MP1 MetroidPrime = null;
        private static Dictionary<String, Image> img = new Dictionary<String, Image>();
        private static IntPtr hHook = IntPtr.Zero;

        public static bool IsRunning {
            get
            {
                return dolphin == null ? false : !dolphin.HasExited;
            }
        }

        public static String GameCode {
            get
            {
                return Encoding.ASCII.GetString(Utils.Read(dolphin, RAMBaseAddr, 6)).Trim('\0');
            }
        }

        public static int GameVersion
        {
            get
            {
                return (int)Utils.Read(dolphin, RAMBaseAddr + 7, 1)[0];
            }
        }

        private static bool IsValidGameCode(String s, int i=0)
        {
            if (s == "") return false;
            if (s.Length != 6) return false;
            if (i == 6) return true;
            if ((s[i] >= 'A' && s[i] <= 'Z') || (s[i] >= '0' && s[i] <= '9')) return IsValidGameCode(s, i + 1);
            else return false;
        }

        internal static bool Init()
        {
            dolphin = Process.GetProcessesByName("dolphin").Length == 0 ? null : Process.GetProcessesByName("dolphin").First();
            if (dolphin == null)
                return false;
            Is32BitProcess = dolphin.MainModule.BaseAddress.ToInt64() < UInt32.MaxValue;
            return true;
        }

        internal static bool GameInit()
        {
            var windowHandles = WinAPI.Imports.GetAllChildWindowHandlesByWindowTitles(dolphin);
            GameWindowHandle = IntPtr.Zero;
            foreach (var wH in windowHandles)
            {
                if (wH.Value.Count(new Func<char, bool>((c) => c == '|')) == 5)
                    GameWindowHandle = wH.Key;
                if (GameWindowHandle != IntPtr.Zero)
                    break;
                Thread.Sleep(1);
            }
            if (GameWindowHandle == IntPtr.Zero)
                return false;
            RAMBaseAddr = 0;
            var MaxAddress = Is32BitProcess ? Int32.MaxValue : Int64.MaxValue;
            long address = 0;
            Utils.MEMORY_BASIC_INFORMATION m;
            do
            {
                Thread.Sleep(1);
                m = Utils.CS_VirtualQuery(dolphin, address);
                if ((m.AllocationBase == IntPtr.Zero && m.Protect == Utils.AllocationProtectEnum.PAGE_NOACCESS) ||
                    m.Type != Utils.TypeEnum.MEM_MAPPED ||
                    m.AllocationProtect != Utils.AllocationProtectEnum.PAGE_READWRITE ||
                    m.State == Utils.StateEnum.MEM_FREE ||
                    m.RegionSize.ToInt64() != 0x2000000)
                    goto GameInit_END;
                RAMBaseAddr = Is32BitProcess ? m.AllocationBase.ToInt32() : m.AllocationBase.ToInt64();
                if (!IsValidGameCode(GameCode))
                {
                    RAMBaseAddr = 0;
                    goto GameInit_END;
                }
                break;
GameInit_END:
                if (address == (long)m.BaseAddress + (long)m.RegionSize)
                    break;
                address = (long)m.BaseAddress + (long)m.RegionSize;
            } while (address <= MaxAddress);
            return RAMBaseAddr != 0;
        }

        internal static KeyValuePair<Point,Size> GetDolphinGameWindowPosAndSize()
        {
            return WinAPI.Imports.GetWindowPosAndSize(GameWindowHandle);
        }

        internal static void AttachWindow(IntPtr hwnd)
        {
            if (!IsRunning)
                return;
            Prime.WinAPI.Imports.SetWindowLongPtr(hwnd, Prime.WinAPI.Imports.GWL_STYLE, new IntPtr(Prime.WinAPI.Imports.GetWindowLongPtr(hwnd, Prime.WinAPI.Imports.GWL_STYLE).ToInt32() | Prime.WinAPI.Imports.WS_CHILD));
            Prime.WinAPI.Imports.SetWindowLongPtr(hwnd, Prime.WinAPI.Imports.GWL_EXSTYLE, new IntPtr(Prime.WinAPI.Imports.GetWindowLongPtr(hwnd, Prime.WinAPI.Imports.GWL_EXSTYLE).ToInt32() | Prime.WinAPI.Imports.WS_EX_LAYERED | Prime.WinAPI.Imports.WS_EX_TRANSPARENT));
            Prime.WinAPI.Imports.SetParentWindow(hwnd, GameWindowHandle);
        }

        internal static void InitMP()
        {
            MetroidPrime = null;
            if (GameCode.Substring(0, 3) == "GM8")
            {
                if (GameCode[3] == 'E')
                {
                    if (GameVersion == 0)
                        MetroidPrime = new MP1_NTSC_1_00();
                    if (GameVersion == 2)
                        MetroidPrime = new MP1_NTSC_1_02();
                    if (GameVersion == 48)
                        MetroidPrime = new MP1_NTSC_K();
                }
                if (GameCode[3] == 'P')
                    MetroidPrime = new MP1_PAL();
                if (GameCode[3] == 'J')
                    MetroidPrime = new MP1_NTSC_J();
            }
            if (GameCode.Substring(0, 2) == "R3")
            {
                // Trilogy
                if (GameCode[2] == 'M')
                {
                    if (GameCode[3] == 'E')
                        MetroidPrime = new MPT_MP1_NTSC_U();
                    if (GameCode[3] == 'P')
                        MetroidPrime = new MPT_MP1_PAL();
                }
                if(GameCode[2] == 'I')
                    if (GameCode[3] == 'J')
                        MetroidPrime = new MPT_MP1_NTSC_J();
            }
        }

        internal static void InitTracker(Form form)
        {
            img.Add("Missiles", ImageUtils.MakeOutline(Image.FromFile(@"img\missilelauncher.png"), Color.Black, 3));
            img.Add("Morph Ball", ImageUtils.MakeOutline(Image.FromFile(@"img\morphball.png"), Color.Black, 3));
            img.Add("Morph Ball Bombs", ImageUtils.MakeOutline(Image.FromFile(@"img\morphballbomb.png"), Color.Black, 3));
            img.Add("Power Bombs", ImageUtils.MakeOutline(Image.FromFile(@"img\powerbomb.png"), Color.Black, 3));
            img.Add("Boost Ball", ImageUtils.MakeOutline(Image.FromFile(@"img\boostball.png"), Color.Black, 3));
            img.Add("Spider Ball", ImageUtils.MakeOutline(Image.FromFile(@"img\spiderball.png"), Color.Black, 3));
            img.Add("Space Jump Boots", ImageUtils.MakeOutline(Image.FromFile(@"img\spacejumpboots.png"), Color.Black, 3));
            img.Add("Varia Suit", ImageUtils.MakeOutline(Image.FromFile(@"img\variasuit.png"), Color.Black, 3));
            img.Add("Gravity Suit", ImageUtils.MakeOutline(Image.FromFile(@"img\gravitysuit.png"), Color.Black, 3));
            img.Add("Phazon Suit", ImageUtils.MakeOutline(Image.FromFile(@"img\phazonsuit.png"), Color.Black, 3));
            img.Add("Wave Beam", ImageUtils.MakeOutline(Image.FromFile(@"img\wavebeam.png"), Color.Black, 3));
            img.Add("Ice Beam", ImageUtils.MakeOutline(Image.FromFile(@"img\icebeam.png"), Color.Black, 3));
            img.Add("Plasma Beam", ImageUtils.MakeOutline(Image.FromFile(@"img\plasmabeam.png"), Color.Black, 3));
            img.Add("Charge Beam", ImageUtils.MakeOutline(Image.FromFile(@"img\chargebeam.png"), Color.Black, 3));
            img.Add("Grapple Beam", ImageUtils.MakeOutline(Image.FromFile(@"img\grapplebeam.png"), Color.Black, 3));
            img.Add("Super Missile", ImageUtils.MakeOutline(Image.FromFile(@"img\supermissile.png"), Color.Black, 3));
            img.Add("Wavebuster", ImageUtils.MakeOutline(Image.FromFile(@"img\wavebuster.png"), Color.Black, 3));
            img.Add("Ice Spreader", ImageUtils.MakeOutline(Image.FromFile(@"img\icespreader.png"), Color.Black, 3));
            img.Add("Flamethrower", ImageUtils.MakeOutline(Image.FromFile(@"img\flamethrower.png"), Color.Black, 3));
            img.Add("Thermal Visor", ImageUtils.MakeOutline(Image.FromFile(@"img\thermalvisor.png"), Color.Black, 3));
            img.Add("XRay Visor", ImageUtils.MakeOutline(Image.FromFile(@"img\xrayvisor.png"), Color.Black, 3));
            img.Add("Artifacts", ImageUtils.MakeOutline(Image.FromFile(@"img\artifacts.png"), Color.Black, 3));
            form.Hide();
            form.FormBorderStyle = FormBorderStyle.None;
            form.BackColor = form.TransparencyKey;
            form.SetBounds(0, 0, 0, 0, BoundsSpecified.Location);
            Dolphin.AttachWindow(form.Handle);
            form.Show();
        }

        internal static void UpdateTracker(Graphics g)
        {
            KeyValuePair<Point, Size> gameWindowPosAndSize = Dolphin.GetDolphinGameWindowPosAndSize();
            if (MetroidPrime.CGameState == -1)
                return;
            if (Dolphin.MetroidPrime.IGT > 0)
            {
                DrawIGT(g, gameWindowPosAndSize.Value);
                if(!MetroidPrime.IsSwitchingState)
                    foreach (KeyValuePair<String, Image> kvp in img)
                        DrawUpgradeIcon(g, gameWindowPosAndSize.Value, kvp.Key);
            }

            g.Save();
            g.Flush();
        }

        internal static void UpdateTrackerInfo(MPItemTracker.Form1 form)
        {
            KeyValuePair<Point, Size> gameWindowPosAndSize = Dolphin.GetDolphinGameWindowPosAndSize();
            if (MetroidPrime.CGameState == -1)
                return;
            form.SetWindowPosAndSize(new Point(0, 0), gameWindowPosAndSize.Value);
        }

        internal static void DrawIGT(Graphics g, Size windowSize)
        {
            float ratio = (float)windowSize.Width / windowSize.Height;
            int fontSize = (int)(15.7894735f * ratio);
            Font _Font = new Font("Arial", fontSize);
            String IGT = MetroidPrime.IGTAsStr;
            int IGT_txtW = (int)g.MeasureString(IGT, _Font).Width;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString(IGT, _Font, Brushes.White, windowSize.Width / 2 - IGT_txtW / 2, (int)(0.07f * windowSize.Height));
        }

        internal static void DrawUpgradeIcon(Graphics g, Size windowSize, String upgrade_title)
        {
            float ratio = (float)windowSize.Width / windowSize.Height;
            String displayText = "x";
            int artifactCount = 0;
            int imgSize = (int)((float)windowSize.Width * 0.0282906581f);
            int fontSize = (int)(15.7894735f * (1.0f - (1.0f / imgSize)));
            if (ratio < 1.4f)
                imgSize = (int)((float)windowSize.Width * 0.0402906581f);
            if (imgSize > 64)
                imgSize = 64;
            Font _Font = new Font("Arial", fontSize);
            int center_x = windowSize.Width / 2;
            int x = Int32.MinValue;

            if (upgrade_title == "Missiles")
            {
                if (MetroidPrime.HaveMissiles)
                {
                    displayText += MetroidPrime.MaxMissiles / 5;
                    x = center_x - imgSize * 11;
                }
            }
            if (upgrade_title == "Morph Ball")
            {
                if (MetroidPrime.HaveMorphBall)
                    x = center_x - imgSize * 10;
            }
            if (upgrade_title == "Morph Ball Bombs")
            {
                if (MetroidPrime.HaveMorphBallBombs)
                    x = center_x - imgSize * 9 + 5;
            }
            if (upgrade_title == "Power Bombs")
            {
                if (MetroidPrime.HavePowerBombs)
                {
                    displayText += MetroidPrime.MaxPowerBombs;
                    x = center_x - imgSize * 8 + 10;
                }
            }
            if (upgrade_title == "Boost Ball")
            {
                if (MetroidPrime.HaveBoostBall)
                    x = center_x - imgSize * 7 + 15;
            }
            if (upgrade_title == "Spider Ball")
            {
                if (MetroidPrime.HaveSpiderBall)
                    x = center_x - imgSize * 6 + 20;
            }
            if (upgrade_title == "Space Jump Boots")
            {
                if (MetroidPrime.HaveSpaceJumpBoots)
                    x = center_x - imgSize * 5 + 20;
            }
            if (upgrade_title == "Varia Suit")
            {
                if (MetroidPrime.HaveVariaSuit)
                    x = center_x - imgSize * 4 + 20;
            }
            if (upgrade_title == "Gravity Suit")
            {
                if (MetroidPrime.HaveGravitySuit)
                    x = center_x - imgSize * 3 + 20;
            }
            if (upgrade_title == "Phazon Suit")
            {
                if (MetroidPrime.HavePhazonSuit)
                    x = center_x - imgSize * 2 + 20;
            }
            if (upgrade_title == "Wave Beam")
            {
                if (MetroidPrime.HaveWaveBeam)
                    x = center_x - imgSize + 20;
            }
            if (upgrade_title == "Ice Beam")
            {
                if (MetroidPrime.HaveIceBeam)
                    x = center_x + 20;
            }
            if (upgrade_title == "Plasma Beam")
            {
                if (MetroidPrime.HavePlasmaBeam)
                    x = center_x + imgSize + 20;
            }
            if (upgrade_title == "Charge Beam")
            {
                if (MetroidPrime.HaveChargeBeam)
                    x = center_x + imgSize * 2 + 20;
            }
            if (upgrade_title == "Grapple Beam")
            {
                if (MetroidPrime.HaveGrappleBeam)
                    x = center_x + imgSize * 3 + 20;
            }
            if (upgrade_title == "Super Missile")
            {
                if (MetroidPrime.HaveSuperMissile)
                    x = center_x + imgSize * 4 + 20;
            }
            if (upgrade_title == "Wavebuster")
            {
                if (MetroidPrime.HaveWavebuster)
                    x = center_x + imgSize * 5 + 15;
            }
            if (upgrade_title == "Ice Spreader")
            {
                if (MetroidPrime.HaveIceSpreader)
                    x = center_x + imgSize * 6 + 10;
            }
            if (upgrade_title == "Flamethrower")
            {
                if (MetroidPrime.HaveFlamethrower)
                    x = center_x + imgSize * 7 + 5;
            }
            if (upgrade_title == "Thermal Visor")
            {
                if (MetroidPrime.HaveThermalVisor)
                    x = center_x + imgSize * 8 + 5;
            }
            if (upgrade_title == "XRay Visor")
            {
                if (MetroidPrime.HaveXRayVisor)
                    x = center_x + imgSize * 9 + 10;
            }

            if (upgrade_title == "Artifacts")
            {
                for (int i = 0; i < 12; i++)
                    if (MetroidPrime.Artifacts(i))
                        artifactCount++;
                if (artifactCount > 0)
                {
                    displayText += artifactCount;
                    x = center_x + imgSize * 10 + 15;
                }
            }

            if (x == Int32.MinValue)
                return;

            if (!img.ContainsKey(upgrade_title))
                return;

            g.DrawImage(img[upgrade_title], x, MetroidPrime.IsMorphed ? windowSize.Height - imgSize * 2 - 18 - fontSize : 5, imgSize, imgSize);

            if(displayText != "x")
                g.DrawString(displayText, _Font, Brushes.White, x + 0.6f * imgSize, MetroidPrime.IsMorphed ? windowSize.Height - imgSize - 15 - fontSize : 5 + imgSize);
        }

        internal static Byte[] Read(long gc_address, int size, bool BigEndian = false)
        {
            try
            {
                long pc_address = RAMBaseAddr + (gc_address - Constants.GC.RAMBaseAddress);
                byte[] datas = Utils.Read(dolphin, pc_address, size);
                return BigEndian ? datas.Reverse().ToArray() : datas;
            }
            catch
            {
                return null;
            }
        }

        internal static Byte ReadUInt8(long gc_address)
        {
            byte[] datas = Read(gc_address, 1);
            if (datas == null)
                return 0;
            return datas[0];
        }

        internal static UInt16 ReadUInt16(long gc_address)
        {
            byte[] datas = Read(gc_address, 2, true);
            if (datas == null)
                return 0;
            return BitConverter.ToUInt16(datas, 0);
        }

        internal static UInt32 ReadUInt32(long gc_address)
        {
            byte[] datas = Read(gc_address, 4, true);
            if (datas == null)
                return 0;
            return BitConverter.ToUInt32(datas, 0);
        }

        internal static UInt64 ReadUInt64(long gc_address)
        {
            byte[] datas = Read(gc_address, 8, true);
            if (datas == null)
                return 0;
            return BitConverter.ToUInt64(datas, 0);
        }

        internal static SByte ReadInt8(long gc_address)
        {
            byte[] datas = Read(gc_address, 1);
            if (datas == null)
                return 0;
            return (SByte)datas[0];
        }

        internal static Int16 ReadInt16(long gc_address)
        {
            byte[] datas = Read(gc_address, 2, true);
            if (datas == null)
                return 0;
            return BitConverter.ToInt16(datas, 0);
        }

        internal static Int32 ReadInt32(long gc_address)
        {
            byte[] datas = Read(gc_address, 4, true);
            if (datas == null)
                return 0;
            return BitConverter.ToInt32(datas, 0);
        }

        internal static Int64 ReadInt64(long gc_address)
        {
            byte[] datas = Read(gc_address, 8, true);
            if (datas == null)
                return 0;
            return BitConverter.ToInt64(datas, 0);
        }

        internal static Single ReadFloat32(long gc_address)
        {
            byte[] datas = Read(gc_address, 4, true);
            if (datas == null)
                return Single.NaN;
            return BitConverter.ToSingle(datas, 0);
        }

        internal static Double ReadFloat64(long gc_address)
        {
            byte[] datas = Read(gc_address, 8, true);
            if (datas == null)
                return Double.NaN;
            return BitConverter.ToDouble(datas, 0);
        }

        internal static void Write(long gc_address, Byte[] datas, bool BigEndian = false)
        {
            try
            {
                long pc_address = RAMBaseAddr + (gc_address - Constants.GC.RAMBaseAddress);
                Utils.Write(dolphin, pc_address, BigEndian ? datas.Reverse().ToArray() : datas);
            }
            catch { }
        }

        internal static void WriteUInt8(long gc_address, Byte value)
        {
            Write(gc_address, new Byte[] { value });
        }

        internal static void WriteUInt16(long gc_address, UInt16 value)
        {
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteUInt32(long gc_address, UInt32 value)
        {
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteUInt64(long gc_address, UInt64 value)
        {
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteInt8(long gc_address, SByte value)
        {
            Write(gc_address, new Byte[] { (Byte)value });
        }

        internal static void WriteInt16(long gc_address, Int16 value)
        {
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteInt32(long gc_address, Int32 value)
        {
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteInt64(long gc_address, Int64 value)
        {
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteFloat32(long gc_address, Single value)
        {
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteFloat64(long gc_address, Double value)
        {
            Write(gc_address, BitConverter.GetBytes(value), true);
        }
    }
}
