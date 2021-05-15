using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;


namespace Imports
{
    public static class Linux
    {
#if LINUX
        #region Native Enums
        public enum Gravity
        {
            ForgetGravity = 0,
            NorthWestGravity = 1,
            NorthGravity = 2,
            NorthEastGravity = 3,
            WestGravity = 4,
            CenterGravity = 5,
            EastGravity = 6,
            SouthWestGravity = 7,
            SouthGravity = 8,
            SouthEastGravity = 9,
            StaticGravity = 10
        }

        public enum MapState
        {
            IsUnmapped = 0,
            IsUnviewable = 1,
            IsViewable = 2
        }
        #endregion

        #region Native Structs
        [StructLayout(LayoutKind.Sequential)]
        public struct XWindowAttributes
        {
            public int x;
            public int y;
            public int width;
            public int height;
            public int border_width;
            public int depth;
            public IntPtr visual;
            public IntPtr root;
            public int c_class;
            public Gravity bit_gravity;
            public Gravity win_gravity;
            public int backing_store;
            public IntPtr backing_planes;
            public IntPtr backing_pixel;
            public bool save_under;
            public IntPtr colormap;
            public bool map_installed;
            public MapState map_state;
            public IntPtr all_event_masks;
            public IntPtr your_event_mask;
            public IntPtr do_not_propagate_mask;
            public bool override_direct;
            public IntPtr screen;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct XSizeHints
        {
            public IntPtr flags;
            public int x;
            public int y;
            public int width;
            public int height;
            public int min_width;
            public int min_height;
            public int max_width;
            public int max_height;
            public int width_inc;
            public int height_inc;
            public int min_aspect_x;
            public int min_aspect_y;
            public int max_aspect_x;
            public int max_aspect_y;
            public int base_width;
            public int base_height;
            public int win_gravity;
        }
        #endregion

        #region Native Functions
        [DllImport("libX11")]
        static extern IntPtr XDefaultRootWindow(IntPtr display);

        [DllImport("libX11", EntryPoint = "XQueryTree")]
        static extern int XQueryTree(IntPtr display, IntPtr w, out IntPtr root_return, out IntPtr parent_return, out IntPtr[] children_return, out int nchildren_return);

        [DllImport("libX11", EntryPoint = "XFetchName")]
        static extern int XFetchName(IntPtr display, IntPtr w, out string window_name_return);

        [DllImport("libX11", EntryPoint = "XOpenDisplay")]
        static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport("libX11", EntryPoint = "XCloseDisplay")]
        static extern int XCloseDisplay(IntPtr display);

        [DllImport("libX11", EntryPoint = "XInternAtom")]
        static extern IntPtr XInternAtom(IntPtr display, string atom_name, bool only_if_exists);

        [DllImport("libX11", EntryPoint = "XGetWindowAttributes")]
        static extern int XGetWindowAttributes(IntPtr display, IntPtr window, ref XWindowAttributes attributes);

        [DllImport("libX11", EntryPoint = "XGetWindowProperty")]
        static extern int XGetWindowProperty(IntPtr display, IntPtr window, IntPtr atom, IntPtr long_offset, IntPtr long_length, bool delete, IntPtr req_type, out IntPtr actual_type, out int actual_format, out IntPtr nitems, out IntPtr bytes_after, ref IntPtr prop);

        [DllImport("libX11", EntryPoint = "XGetWMNormalHints")]
        static extern int XGetWMNormalHints(IntPtr display, IntPtr window, ref XSizeHints hints, out IntPtr supplied_return);

        [DllImport("libX11", EntryPoint = "XFree")]
        public extern static int XFree(IntPtr data);

        [DllImport("libX11", EntryPoint = "XReparentWindow")]
        public extern static int XReparentWindow(IntPtr display, IntPtr window, IntPtr parent, int x, int y);

        [DllImport("libX11", EntryPoint = "XSetWindowBackground")]
        public extern static int XSetWindowBackground(IntPtr display, IntPtr window, IntPtr background);
        #endregion

        static IntPtr display = IntPtr.Zero;
#endif

        public static void Init()
        {
#if LINUX
            if (display == IntPtr.Zero)
            {
                display = XOpenDisplay(IntPtr.Zero);
                if (display == IntPtr.Zero)
                    throw new Exception("Couldn't get display!");
            }
#else
            throw new NotImplementedException();
#endif
        }

        public static void DeInit()
        {
#if LINUX
            if (display != IntPtr.Zero)
            {
                XCloseDisplay(display);
                display = IntPtr.Zero;
            }
#else
            throw new NotImplementedException();
#endif
        }

        public static VirtualMemoryInformation[] EnumerateVirtualMemorySpaces(Process proc)
        {
#if LINUX
            List<VirtualMemoryInformation> virtual_mem_entries = new List<VirtualMemoryInformation>();
            String[] virtual_mem_entry_lines = File.ReadAllLines("/proc/" + proc.Id + "/maps");
            String[] values = null;
            String[] address_space = null;
            VirtualMemoryPermissions perms = 0;

            foreach (var virtual_mem_entry_line in virtual_mem_entry_lines)
            {
                values = virtual_mem_entry_line.Split(' ');
                address_space = values[0].Split('-');
                perms = 0;

                if (values[1][0] == 'r')
                    perms = VirtualMemoryPermissions.READ;
                if (values[1][1] == 'w')
                    perms |= VirtualMemoryPermissions.WRITE;
                if (values[1][2] == 'x')
                    perms |= VirtualMemoryPermissions.EXECUTE;

                virtual_mem_entries.Add(new VirtualMemoryInformation(
                    Convert.ToInt64("0x" + address_space[0], 16),
                    Convert.ToInt64("0x" + address_space[1], 16),
                    perms,
                    values[1][3] == 'p',
                    values[1][3] == 's'
                ));
                Thread.Sleep(1);
            }
            return virtual_mem_entries.ToArray();
#else
            throw new NotImplementedException();
#endif
        }

        public static byte[] ReadProcessMemory(Process proc, long address, int len)
        {
#if LINUX
            using (var reader = new BinaryReader(File.OpenRead("/proc/" + proc.Id + "/mem")))
            {
                reader.BaseStream.Position = address;
                return reader.ReadBytes(len);
            }
#else
            throw new NotImplementedException();
#endif
        }

        public static void WriteProcessMemory(Process dolphin, long address, byte[] datas)
        {
#if LINUX
            using (var writer = new BinaryWriter(File.OpenWrite("/proc/" + dolphin.Id + "/mem")))
            {
                writer.BaseStream.Position = address;
                writer.Write(datas);
            }
#else
            throw new NotImplementedException();
#endif
        }

        public static IntPtr[] FindChildWindows(IntPtr window, ref List<IntPtr> windows)
        {
#if LINUX
            IntPtr[] childWindows = new IntPtr[0];
            XQueryTree(display, window, out IntPtr rootWindow, out IntPtr parentWindow, out childWindows, out int childWindowsLength);
            childWindows = new IntPtr[childWindowsLength];
            XQueryTree(display, window, out rootWindow, out parentWindow, out childWindows, out childWindowsLength);

            windows.Add(window);
            windows.AddRange(childWindows);

            foreach (var childWindow in childWindows)
            {
                FindChildWindows(childWindow, ref windows);
            }

            windows.TrimExcess();

            return windows.ToArray();
#else
            throw new NotImplementedException();
#endif
        }

        public static IntPtr FindWindow(string title)
        {
#if LINUX
            List<IntPtr> windows = new List<IntPtr>();
            IntPtr ret = IntPtr.Zero;

            if (FindChildWindows(XDefaultRootWindow(display), ref windows).Length == 0)
                return IntPtr.Zero;

            foreach (var window in windows)
            {
                if (GetWindowTitle(window) == title)
                {
                    ret = window;
                    break;
                }
            }

            return ret;
#else
            throw new NotImplementedException();
#endif
        }

        public static IntPtr FindWindowByPID(int pid)
        {
#if LINUX
            List<IntPtr> windows = new List<IntPtr>();
            IntPtr ret = IntPtr.Zero;
            IntPtr net_wm_pid = IntPtr.Zero;
            IntPtr cardinal_atom = IntPtr.Zero;
            IntPtr type = IntPtr.Zero;
            IntPtr win_pid = IntPtr.Zero;

            if (FindChildWindows(XDefaultRootWindow(display), ref windows).Length == 0)
                return IntPtr.Zero;

            net_wm_pid = XInternAtom(display, "_NET_WM_PID", false);
            cardinal_atom = XInternAtom(display, "CARDINAL", false);

            foreach (var window in windows)
            {
                XGetWindowProperty(display, window, net_wm_pid, new IntPtr(0), new IntPtr(65536), false, cardinal_atom, out type, out int format, out IntPtr nitems, out IntPtr after, ref win_pid);
                if (GetWindowTitle(window) != null &&
                    win_pid != IntPtr.Zero &&
                    Marshal.ReadIntPtr(win_pid).ToInt64() == pid)
                {
                    ret = window;
                    break;
                }
            }

            return ret;
#else
            throw new NotImplementedException();
#endif
        }

        public static Point GetWindowPosition(IntPtr window)
        {
#if LINUX
            XSizeHints size_hints = new XSizeHints();
            XGetWMNormalHints(display, window, ref size_hints, out IntPtr supplied_return);
            return new Point(size_hints.x, size_hints.y);
#else
            throw new NotImplementedException();
#endif
        }

        public static Size GetWindowSize(IntPtr window)
        {
#if LINUX
            XSizeHints size_hints = new XSizeHints();
            XGetWMNormalHints(display, window, ref size_hints, out IntPtr supplied_return);
            return new Size(size_hints.width, size_hints.height);
#else
            throw new NotImplementedException();
#endif
        }

        public static String GetWindowTitle(IntPtr window)
        {
#if LINUX
            try {
                XFetchName(display, window, out string window_name);
                return window_name;
            } catch { 
                return "";
            }
#else
            throw new NotImplementedException();
#endif
        }

        public static void AttachWindow(IntPtr parent, IntPtr child)
        {
#if LINUX
            XSetWindowBackground(display, child, new IntPtr(Color.Pink.ToArgb()));
            XReparentWindow(display, child, parent, 0, 0);
#else
            throw new NotImplementedException();
#endif
        }
    }
}