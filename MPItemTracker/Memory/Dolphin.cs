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

        public static bool IsRunning {
            get
            {
                return dolphin == null ? true : !dolphin.HasExited;
            }
        }

        public static String GameCode {
            get
            {
                return Encoding.ASCII.GetString(Utils.Read(dolphin, RAMBaseAddr, 6)).Trim('\0');
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
				if (m.AllocationBase == IntPtr.Zero && m.Protect == Utils.AllocationProtectEnum.PAGE_NOACCESS)
                {
                    if (address == (long)m.BaseAddress + (long)m.RegionSize)
                        break;
                    address = (long)m.BaseAddress + (long)m.RegionSize;
                    continue;
                }
                if (m.Type != Utils.TypeEnum.MEM_MAPPED)
                {
                    if (address == (long)m.BaseAddress + (long)m.RegionSize)
                        break;
                    address = (long)m.BaseAddress + (long)m.RegionSize;
                    continue;
                }
                if (m.AllocationProtect != Utils.AllocationProtectEnum.PAGE_READWRITE)
                {
                    if (address == (long)m.BaseAddress + (long)m.RegionSize)
                        break;
                    address = (long)m.BaseAddress + (long)m.RegionSize;
                    continue;
                }
                if (m.State == Utils.StateEnum.MEM_FREE)
                {
                    if (address == (long)m.BaseAddress + (long)m.RegionSize)
                        break;
                    address = (long)m.BaseAddress + (long)m.RegionSize;
                    continue;
                }
                if (m.RegionSize.ToInt64() <= 0x20000)
                {
                    if (address == (long)m.BaseAddress + (long)m.RegionSize)
                        break;
                    address = (long)m.BaseAddress + (long)m.RegionSize;
                    continue;
                }
                RAMBaseAddr = Is32BitProcess ? m.AllocationBase.ToInt32() : m.AllocationBase.ToInt64();
                if (!IsValidGameCode(GameCode))
                {
                    RAMBaseAddr = 0;
                    if (address == (long)m.BaseAddress + (long)m.RegionSize)
                        break;
                    address = (long)m.BaseAddress + (long)m.RegionSize;
                    continue;
                }
                break;
            } while (address <= MaxAddress);
            return RAMBaseAddr != 0;
        }

        internal static KeyValuePair<Point,Size> GetDolphinGameWindowPosAndSize()
        {
            return WinAPI.Imports.GetWindowPosAndSize(GameWindowHandle);
        }

        internal static void AttachWindow(IntPtr hwnd)
        {
            Prime.WinAPI.Imports.SetWindowLongPtr(hwnd, Prime.WinAPI.Imports.GWL_STYLE, new IntPtr(Prime.WinAPI.Imports.GetWindowLongPtr(hwnd, Prime.WinAPI.Imports.GWL_STYLE).ToInt32() | Prime.WinAPI.Imports.WS_CHILD));
            Prime.WinAPI.Imports.SetWindowLongPtr(hwnd, Prime.WinAPI.Imports.GWL_EXSTYLE, new IntPtr(Prime.WinAPI.Imports.GetWindowLongPtr(hwnd, Prime.WinAPI.Imports.GWL_EXSTYLE).ToInt32() | Prime.WinAPI.Imports.WS_EX_LAYERED | Prime.WinAPI.Imports.WS_EX_TRANSPARENT));
            Prime.WinAPI.Imports.SetParentWindow(hwnd, GameWindowHandle);
        }

        internal static void InitMP()
        {
            MetroidPrime = null;
            if(GameCode[3] == 'E')
                MetroidPrime = new MP1_NTSC_1_00();
            if (GameCode[3] == 'P')
                MetroidPrime = new MP1_PAL();
        }

        internal static void InitTracker(Form form)
        {
            img.Add("Missiles", Image.FromFile(@"img\missilelauncher.png"));
            img.Add("Morph Ball", Image.FromFile(@"img\morphball.png"));
            img.Add("Morph Ball Bombs", Image.FromFile(@"img\morphballbomb.png"));
            img.Add("Power Bombs", Image.FromFile(@"img\powerbomb.png"));
            img.Add("Boost Ball", Image.FromFile(@"img\boostball.png"));
            img.Add("Spider Ball", Image.FromFile(@"img\spiderball.png"));
            img.Add("Space Jump Boots", Image.FromFile(@"img\spacejumpboots.png"));
            img.Add("Varia Suit", Image.FromFile(@"img\variasuit.png"));
            img.Add("Gravity Suit", Image.FromFile(@"img\gravitysuit.png"));
            img.Add("Phazon Suit", Image.FromFile(@"img\phazonsuit.png"));
            img.Add("Wave Beam", Image.FromFile(@"img\wavebeam.png"));
            img.Add("Ice Beam", Image.FromFile(@"img\icebeam.png"));
            img.Add("Plasma Beam", Image.FromFile(@"img\plasmabeam.png"));
            img.Add("Charge Beam", Image.FromFile(@"img\chargebeam.png"));
            img.Add("Grapple Beam", Image.FromFile(@"img\grapplebeam.png"));
            img.Add("Super Missile", Image.FromFile(@"img\supermissile.png"));
            img.Add("Wavebuster", Image.FromFile(@"img\wavebuster.png"));
            img.Add("Ice Spreader", Image.FromFile(@"img\icespreader.png"));
            img.Add("Flamethrower", Image.FromFile(@"img\flamethrower.png"));
            img.Add("Thermal Visor", Image.FromFile(@"img\thermalvisor.png"));
            img.Add("XRay Visor", Image.FromFile(@"img\xrayvisor.png"));
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
            if (MetroidPrime.CWorld == -1)
                return;
            DrawIGT(g, gameWindowPosAndSize.Value);
            foreach(KeyValuePair<String, Image> kvp in img)
                DrawUpgradeIcon(g, gameWindowPosAndSize.Value, kvp.Key);
        }

        internal static void UpdateTrackerInfo(MPItemTracker.Form1 form)
        {
            KeyValuePair<Point, Size> gameWindowPosAndSize = Dolphin.GetDolphinGameWindowPosAndSize();
            if (MetroidPrime.CWorld == -1)
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
            int imgSize = (int)((float)windowSize.Width * 0.0442906581f);
            if(upgrade_title == "Missiles")
            {
                if(MetroidPrime.HaveMissiles)
                    g.DrawImage(img[upgrade_title], 25, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Morph Ball")
            {
                if (MetroidPrime.HaveMorphBall)
                    g.DrawImage(img[upgrade_title], 25 + imgSize, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Morph Ball Bombs")
            {
                if (MetroidPrime.HaveMorphBallBombs)
                    g.DrawImage(img[upgrade_title], 35 + imgSize * 2, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Power Bombs")
            {
                if (MetroidPrime.HavePowerBombs)
                    g.DrawImage(img[upgrade_title], 45 + imgSize * 3, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Boost Ball")
            {
                if (MetroidPrime.HaveBoostBall)
                    g.DrawImage(img[upgrade_title], 55 + imgSize * 4, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Spider Ball")
            {
                if (MetroidPrime.HaveSpiderBall)
                    g.DrawImage(img[upgrade_title], 65 + imgSize * 5, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Space Jump Boots")
            {
                if (MetroidPrime.HaveSpaceJumpBoots)
                    g.DrawImage(img[upgrade_title], 65 + imgSize * 6, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Varia Suit")
            {
                if (MetroidPrime.HaveVariaSuit)
                    g.DrawImage(img[upgrade_title], 65 + imgSize * 7, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Gravity Suit")
            {
                if (MetroidPrime.HaveGravitySuit)
                    g.DrawImage(img[upgrade_title], 70 + imgSize * 8, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Phazon Suit")
            {
                if (MetroidPrime.HavePhazonSuit)
                    g.DrawImage(img[upgrade_title], 75 + imgSize * 9, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Wave Beam")
            {
                if (MetroidPrime.HaveWaveBeam)
                    g.DrawImage(img[upgrade_title], 75 + imgSize * 10, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Ice Beam")
            {
                if (MetroidPrime.HaveIceBeam)
                    g.DrawImage(img[upgrade_title], 70 + imgSize * 11, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Plasma Beam")
            {
                if (MetroidPrime.HavePlasmaBeam)
                    g.DrawImage(img[upgrade_title], 65 + imgSize * 12, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Charge Beam")
            {
                if (MetroidPrime.HaveChargeBeam)
                    g.DrawImage(img[upgrade_title], 65 + imgSize * 13, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Grapple Beam")
            {
                if (MetroidPrime.HaveGrappleBeam)
                    g.DrawImage(img[upgrade_title], 65 + imgSize * 14, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Super Missile")
            {
                if (MetroidPrime.HaveSuperMissile)
                    g.DrawImage(img[upgrade_title], 65 + imgSize * 15, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Wavebuster")
            {
                if (MetroidPrime.HaveWavebuster)
                    g.DrawImage(img[upgrade_title], 55 + imgSize * 16, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Ice Spreader")
            {
                if (MetroidPrime.HaveIceSpreader)
                    g.DrawImage(img[upgrade_title], 45 + imgSize * 17, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Flamethrower")
            {
                if (MetroidPrime.HaveFlamethrower)
                    g.DrawImage(img[upgrade_title], 35 + imgSize * 18, 5, imgSize, imgSize);
            }
            if (upgrade_title == "Thermal Visor")
            {
                if (MetroidPrime.HaveThermalVisor)
                    g.DrawImage(img[upgrade_title], 30 + imgSize * 19, 5, imgSize, imgSize);
            }
            if (upgrade_title == "XRay Visor")
            {
                if (MetroidPrime.HaveXRayVisor)
                    g.DrawImage(img[upgrade_title], 40 + imgSize * 20, 5, imgSize, imgSize);
            }
        }

        internal static Byte[] Read(long gc_address, int size, bool BigEndian=false)
        {
            if (!IsRunning) return null;
            long pc_address = RAMBaseAddr + (gc_address - Constants.GC.RAMBaseAddress);
            byte[] datas = Utils.Read(dolphin, pc_address, size);
            return BigEndian ? datas.Reverse().ToArray() : datas;
        }

        internal static Byte ReadUInt8(long gc_address)
        {
            if (!IsRunning) return 0;
            return Read(gc_address, 1)[0];
        }

        internal static UInt16 ReadUInt16(long gc_address)
        {
            if (!IsRunning) return 0;
            return BitConverter.ToUInt16(Read(gc_address, 2, true), 0);
        }

        internal static UInt32 ReadUInt32(long gc_address)
        {
            if (!IsRunning) return 0;
            return BitConverter.ToUInt32(Read(gc_address, 4, true), 0);
        }

        internal static UInt64 ReadUInt64(long gc_address)
        {
            if (!IsRunning) return 0;
            return BitConverter.ToUInt64(Read(gc_address, 8, true), 0);
        }

        internal static SByte ReadInt8(long gc_address)
        {
            if (!IsRunning) return 0;
            return (SByte)Read(gc_address, 1)[0];
        }

        internal static Int16 ReadInt16(long gc_address)
        {
            if (!IsRunning) return 0;
            return BitConverter.ToInt16(Read(gc_address, 2, true), 0);
        }

        internal static Int32 ReadInt32(long gc_address)
        {
            if (!IsRunning) return 0;
            return BitConverter.ToInt32(Read(gc_address, 4, true), 0);
        }

        internal static Int64 ReadInt64(long gc_address)
        {
            if (!IsRunning) return 0;
            return BitConverter.ToInt64(Read(gc_address, 8, true), 0);
        }

        internal static Single ReadFloat32(long gc_address)
        {
            if (!IsRunning) return Single.NaN;
            return BitConverter.ToSingle(Read(gc_address, 4, true), 0);
        }

        internal static Double ReadFloat64(long gc_address)
        {
            if (!IsRunning) return Double.NaN;
            return BitConverter.ToDouble(Read(gc_address, 8, true), 0);
        }

        internal static void Write(long gc_address, Byte[] datas, bool BigEndian=false)
        {
            if (!IsRunning) return;
            long pc_address = RAMBaseAddr + (gc_address - Constants.GC.RAMBaseAddress);
            Utils.Write(dolphin, pc_address, BigEndian ? datas.Reverse().ToArray() : datas);
        }

        internal static void WriteUInt8(long gc_address, Byte value)
        {
            if (!IsRunning) return;
            Write(gc_address, new Byte[] { value });
        }

        internal static void WriteUInt16(long gc_address, UInt16 value)
        {
            if (!IsRunning) return;
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteUInt32(long gc_address, UInt32 value)
        {
            if (!IsRunning) return;
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteUInt64(long gc_address, UInt64 value)
        {
            if (!IsRunning) return;
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteInt8(long gc_address, SByte value)
        {
            if (!IsRunning) return;
            Write(gc_address, new Byte[] { (Byte)value });
        }

        internal static void WriteInt16(long gc_address, Int16 value)
        {
            if (!IsRunning) return;
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteInt32(long gc_address, Int32 value)
        {
            if (!IsRunning) return;
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteInt64(long gc_address, Int64 value)
        {
            if (!IsRunning) return;
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteFloat32(long gc_address, Single value)
        {
            if (!IsRunning) return;
            Write(gc_address, BitConverter.GetBytes(value), true);
        }

        internal static void WriteFloat64(long gc_address, Double value)
        {
            if (!IsRunning) return;
            Write(gc_address, BitConverter.GetBytes(value), true);
        }
    }
}
