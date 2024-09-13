using Imports;
using MPItemTracker2;
using MPItemTracker2.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Text.Json;
using WindowsInput.Native;
using WindowsInput;

namespace Wrapper
{
    public enum IGTDisplayType
    {
        None,
        WithoutMS,
        WithMS
    };

    public static class Dolphin
    {
        static Process dolphin = null;
        static IntPtr dolphin_window = IntPtr.Zero;
        static bool Is32BitProcess = false;
        static Metroid MetroidPrime = null;
        static String[] pickups_to_show = null;
        static VirtualKeyCode refresh_config_key = default;
        static IGTDisplayType igt_display_type = IGTDisplayType.None;
        static InputSimulator inputSimulator = new InputSimulator();

        internal static bool IsRunning
        {
            get
            {
                return dolphin != null && !dolphin.HasExited;
            }
        }

        internal static String GameCode
        {
            get
            {
                return Encoding.ASCII.GetString(GCMem.Read(0x80000000, 6)).Trim('\0');
            }
        }

        internal static int GameVersion
        {
            get
            {
                return (int)GCMem.Read(0x80000007, 1)[0];
            }
        }

        internal static bool IsWiiGame
        {
            get
            {
                return GCMem.ReadUInt32(0x80000018) == 0x5D1C9EA3;
            }
        }

        internal static bool IsGCGame
        {
            get
            {
                return GCMem.ReadUInt32(0x8000001C) == 0xC2339F3D;
            }
        }

        static bool IsValidGameCode(String s, int i = 0)
        {
            if (s == "") return false;
            if (s.Length != 6) return false;
            if (i == 6) return true;
            if ((s[i] >= 'A' && s[i] <= 'Z') || (s[i] >= '0' && s[i] <= '9')) return IsValidGameCode(s, i + 1);
            else return false;
        }

        internal static bool Init()
        {
            String[] PROCESSES_TO_CHECK = new String[] { "dolphin", "dolphin-emu", "MPR" };
            String window_title = String.Empty;

            foreach (var process in PROCESSES_TO_CHECK)
            {
                dolphin = Process.GetProcessesByName(process).FirstOrDefault();
                if (dolphin != null)
                    break;
            }

            if (dolphin == null)
                return false;

            Is32BitProcess = dolphin.MainModule.BaseAddress.ToInt64() < UInt32.MaxValue;
            dolphin_window = ImportsMgr.FindWindowByPID(dolphin.Id);
            if (dolphin_window == IntPtr.Zero)
                return false;

            window_title = ImportsMgr.GetWindowTitle(dolphin_window);
            if (window_title.Count((c) => c == '|') >= 4 && window_title.Count((c) => c == '|') <= 5)
                return true;
#if LINUX
            List<IntPtr> childWindows = new List<IntPtr>();
            if (ImportsMgr.FindChildWindows(dolphin_window, ref childWindows).Length > 0)
            {
                foreach (var childWindow in childWindows)
                {
                    dolphin_window = childWindow;
                    window_title = ImportsMgr.GetWindowTitle(dolphin_window);
                    if (window_title.Count((c) => c == '|') >= 4 && window_title.Count((c) => c == '|') <= 5)
                        return true;
                }
            }
#endif
            dolphin_window = IntPtr.Zero;
            return false;
        }

        internal static void LoadConfig()
        {
            int i;
            String game_config_filename = String.Empty;
            dynamic config = JsonSerializer.Deserialize<dynamic>(File.ReadAllText(Path.Combine(Program.ExecutableDir, "config.json")));
            try {
                refresh_config_key = KeysUtils.ConvertFromString(config.GetProperty("refresh_config_key").GetString());
                igt_display_type = (IGTDisplayType)Enum.Parse(typeof(IGTDisplayType), config.GetProperty("igt_display_type").GetString());
                if (MetroidPrime != null)
                {
                    if (MetroidPrime.GetType().BaseType == typeof(Prime.Prime))
                    {
                        game_config_filename = "prime.json";
                    }
                    if (MetroidPrime.GetType().BaseType == typeof(Echoes.Echoes))
                    {
                        game_config_filename = "echoes.json";
                    }
                    if (MetroidPrime.GetType().BaseType == typeof(Corruption.Corruption))
                    {
                        game_config_filename = "corruption.json";
                    }
                    if (game_config_filename != String.Empty)
                    {
                        dynamic game_config = JsonSerializer.Deserialize<dynamic>(File.ReadAllText(Path.Combine(Program.ExecutableDir, game_config_filename)));
                        try {
                            var pickups = game_config.GetProperty("pickups");
                            pickups_to_show = new String[pickups.GetArrayLength()];
                            for (i = 0; i < pickups_to_show.Length; i++)
                                pickups_to_show[i] = pickups[i].GetString();
                        } catch {
                            pickups_to_show = null;
                        }
                    }
                }
            } catch {
                refresh_config_key = VirtualKeyCode.F5;
                igt_display_type = IGTDisplayType.WithoutMS;
                pickups_to_show = null;
            }
        }

        internal static bool GameInit()
        {
            long RAMBaseAddr = 0;
            var mmap_entries = ImportsMgr.EnumerateVirtualMemorySpaces(dolphin).Where(mmap_entry =>
                mmap_entry.Size == 0x2000000 &&
                !mmap_entry.IsPrivate &&
                mmap_entry.Permissions == (VirtualMemoryPermissions.READ | VirtualMemoryPermissions.WRITE)
            ).ToArray();
            foreach(var mmap_entry in mmap_entries)
            {
                RAMBaseAddr = mmap_entry.BaseAddress;
                GCMem.Init(dolphin, RAMBaseAddr);
                if (!IsValidGameCode(GameCode))
                {
                    RAMBaseAddr = 0;
                    GCMem.DeInit();
                    continue;
                }
                break;
            }
            return RAMBaseAddr != 0;
        }

        internal static void InitMP()
        {
            MetroidPrime = null;
            if (IsGCGame)
            {
                if (GameCode.Substring(0, 3) == "GM8")
                {
                    if (GameCode[3] == 'E')
                    {
                        if (GameVersion == 0)
                            MetroidPrime = new Prime.MP1_NTSC_0_00();
                        if (GameVersion == 1)
                            MetroidPrime = new Prime.MP1_NTSC_0_01();
                        if (GameVersion == 2)
                            MetroidPrime = new Prime.MP1_NTSC_0_02();
                        if (GameVersion == 48)
                            MetroidPrime = new Prime.MP1_NTSC_K();
                    }
                    if (GameCode[3] == 'J')
                        MetroidPrime = new Prime.MP1_NTSC_J();
                    if (GameCode[3] == 'P')
                        MetroidPrime = new Prime.MP1_PAL();
                }
                if (GameCode.Substring(0, 3) == "G2M")
                {
                    if (GameCode[3] == 'E')
                        MetroidPrime = new Echoes.MP2_NTSC_U();
                    if (GameCode[3] == 'J')
                        MetroidPrime = new Echoes.MP2_NTSC_J();
                    if (GameCode[3] == 'P')
                        MetroidPrime = new Echoes.MP2_PAL();
                }
            }
            if (IsWiiGame)
            {
                UInt32 opcode = GCMem.ReadUInt32(0x8046d340);
                if (GameCode.Substring(0, 2) == "R3")
                {
                    // Trilogy
                    if (GameCode[2] == 'M')
                    {
                        if (GameCode[3] == 'E')
                        {
                            while ((opcode = GCMem.ReadUInt32(0x8046d340)) == 0x38000018)
                            {
                                Thread.Sleep(10);
                            }
                            if (opcode == 0x4e800020)
                            {
                                MetroidPrime = new Prime.MPT_MP1_NTSC_U();
                            }
                            if (opcode == 0x4bff64e1)
                            {
                                MetroidPrime = new Echoes.MPT_MP2_NTSC_U();
                            }
                            // Wait for game to be fully loaded
                            Thread.Sleep(100);
                            if (GCMem.ReadUInt32(0x80576ae8) == 0x7d415378)
                            {
                                MetroidPrime = new Corruption.MPT_MP3_NTSC_U();
                            }
                        }
                        if (GameCode[3] == 'P')
                        {
                            while ((opcode = GCMem.ReadUInt32(0x8046d340)) == 0x7c0000d0)
                            {
                                Thread.Sleep(10);
                            }
                            if (opcode == 0x7c962378)
                            {
                                MetroidPrime = new Prime.MPT_MP1_PAL();
                            }
                            if (opcode == 0x80830000)
                            {
                                MetroidPrime = new Echoes.MPT_MP2_PAL();
                            }
                            // Wait for game to be fully loaded
                            Thread.Sleep(100);
                            if (GCMem.ReadUInt32(0x805795a4) == 0x7d415378)
                            {
                                MetroidPrime = new Corruption.MPT_MP3_PAL();
                            }
                        }
                    }

                    if (GameCode[2] == 'I' && GameCode[3] == 'J')
                    {
                        while ((opcode = GCMem.ReadUInt32(0x8046d340)) == 0x806ddaec)
                        {
                            Thread.Sleep(10);
                        }
                        if (opcode == 0x53687566)
                        {
                            MetroidPrime = new Prime.MPT_MP1_NTSC_J();
                        }
                    }

                    if (GameCode[2] == '2' && GameCode[3] == 'J')
                    {
                        while ((opcode = GCMem.ReadUInt32(0x8046d340)) == 0x801e0000)
                        {
                            Thread.Sleep(10);
                        }
                        if (opcode == 0x936daabc)
                        {
                            MetroidPrime = new Echoes.MPT_MP2_NTSC_J();
                        }
                    }
                }

                if (GameCode.Substring(0, 3) == "RM3")
                {
                    if (GameCode[3] == 'E')
                        MetroidPrime = new Corruption.MP3_NTSC_U();
                    if (GameCode[3] == 'J')
                        MetroidPrime = new Corruption.MP3_NTSC_J();
                    if (GameCode[3] == 'P')
                        MetroidPrime = new Corruption.MP3_PAL();
                }
            }
            LoadConfig();
        }

        internal static void InitTracker(Form form)
        {
            try {
                form.Hide();
                form.FormBorderStyle = FormBorderStyle.None;
                FormUtils.SetFormBGColor(form.TransparencyKey);
                form.SetBounds(0, 0, 0, 0, BoundsSpecified.Location);
                ImportsMgr.AttachWindow(dolphin_window, form.Handle);
                form.Show();
            } catch { }
        }

        static Size CalculateIconsRect(int scr_w, int scr_h)
        {
            float four_third = 4.0f / 3.0f;
            float ratio = (float)scr_w / scr_h;
            if (ratio > four_third)
                return new Size((int)((float)scr_h * four_third), scr_h);
            if (ratio < four_third)
                return new Size(scr_w, (int)((float)scr_w / four_third));
            return new Size(scr_w, scr_h);
        }

        static int CalculateIconYOffset(int imgSize, int fontSize, int scr_h)
        {
            if (MetroidPrime.IsMorphed())
                return (int)(0.94f * scr_h - imgSize - fontSize);
            else
                return 5;
        }

        internal static void UpdateTracker(Graphics g)
        {
            int i, start_index = pickups_to_show.Length / 2 - (1 - pickups_to_show.Length % 2);
            String IGT = "";
            Size windowSize = ImportsMgr.GetWindowSize(dolphin_window);
            Size iconsRectSize = CalculateIconsRect(windowSize.Width, windowSize.Height);
            int imgSize = Math.Min((int)((iconsRectSize.Width - 10) / pickups_to_show.Length), 52);
            int x = (windowSize.Width / 2) - start_index * (imgSize + 5);
            int y = (windowSize.Height - iconsRectSize.Height) / 2 + 5;
            Font IGT_Font = new Font("Arial", (int)(15.7894735f * (4.0f / 3.0f)));
            Font Icon_Font = new Font("Arial", (int)(18.0f * ((float)imgSize / 52.0f)));
            int h = CalculateIconYOffset(imgSize, Icon_Font.Height, iconsRectSize.Height);

            if (MetroidPrime.IsIngame())
            {
                if (igt_display_type != IGTDisplayType.None)
                {
                    IGT = MetroidPrime.IGTAsStr(igt_display_type);
                    DrawIGT(g, IGT_Font, windowSize.Width / 2 - (int)g.MeasureString(IGT, IGT_Font).Width / 2, y + imgSize + (int)IGT_Font.Size, IGT);
                }
                if (!MetroidPrime.IsSwitchingState())
                    for (i = 0; i < pickups_to_show.Length; i++)
                    {
                        DrawUpgradeIcon(g, Icon_Font, pickups_to_show[i], x + (int)(((float)i - 1.5f) * (imgSize + 5)), y + h, imgSize);
                    }
            }

            g.Save();
            g.Flush();
        }

        internal static void DrawIGT(Graphics g, Font _Font, int x, int y, String IGT)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString(IGT, _Font, Brushes.White, x, y);
        }

        internal static void DrawUpgradeIcon(Graphics g, Font _Font, String upgrade_title, int x, int y, int imgSize)
        {
            bool has_pickup = false;
            bool is_unlimited = false;
            String str = "x";
            int pickup_count = 0;
            int str_width = 0;
            Image img = MetroidPrime.GetIcon(upgrade_title);
            if (img == null)
                return;
            pickup_count = MetroidPrime.GetPickupCount(upgrade_title);
            has_pickup = MetroidPrime.HasPickup(upgrade_title);
            is_unlimited = pickup_count == - 1;
            if (MetroidPrime.GetType().BaseType == typeof(Echoes.Echoes))
            {
                if (upgrade_title == "Boost Ball")
                {
                    if (MetroidPrime.HasPickup("Cannon Ball"))
                    {
                        if (pickup_count == 0)
                        {   
                            img = MetroidPrime.GetIcon("Cannon Ball");
                            if (img == null)
                                return;
                            has_pickup = true;
                        }
                        else
                        {
                            str = "C";
                        }
                    }
                }
            }

            if (has_pickup)
            {
                g.DrawImage(img, x, y, imgSize, imgSize);

                if(is_unlimited) {
                    str = "∞";
                    _Font = new Font(_Font.FontFamily, 1.75f * _Font.Size);
                    y -= (int)(0.3f * _Font.Size);
                } else {
                    if (str == "x")
                    {
                        str += pickup_count;
                    }
                }

                if (str != "x0" && str != "x1" && !upgrade_title.StartsWith("Progressive "))
                {
                    str_width = (int)g.MeasureString(str, _Font).Width;
                    g.DrawString(str, _Font, Brushes.White, x + imgSize - str_width, y + imgSize);
                }
            }
        }

        internal static void UpdateTrackerInfo(Form form)
        {
            if (!MetroidPrime.IsIngame())
                return;
            FormUtils.SetWindowPosition(new Point(0, 0));
            FormUtils.SetWindowSize(ImportsMgr.GetWindowSize(dolphin_window));

            if (inputSimulator.InputDeviceState.IsKeyDown(refresh_config_key))
                LoadConfig();

            MetroidPrime.Update();
        }
    }
}
