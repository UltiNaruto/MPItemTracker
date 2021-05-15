using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace Imports
{
    public static class Windows
    {
#if WINDOWS
        #region Native constants
        internal static int WM_PAINT = 0xF;
        internal static int WS_EX_TRANSPARENT = 0x20;
        internal static int WS_EX_LAYERED = 0x80000;
        internal static int WS_CHILD = 0x40000000;
        internal static int GWL_EXSTYLE = (-20);
        internal static int GWL_STYLE = (-16);
        #endregion

        #region Native enums
        [Flags]
        public enum AllocationProtectEnum : uint
        {
            PAGE_EXECUTE = 0x00000010,
            PAGE_EXECUTE_READ = 0x00000020,
            PAGE_EXECUTE_READWRITE = 0x00000040,
            PAGE_EXECUTE_WRITECOPY = 0x00000080,
            PAGE_NOACCESS = 0x00000001,
            PAGE_READONLY = 0x00000002,
            PAGE_READWRITE = 0x00000004,
            PAGE_WRITECOPY = 0x00000008,
            PAGE_GUARD = 0x00000100,
            PAGE_NOCACHE = 0x00000200,
            PAGE_WRITECOMBINE = 0x00000400
        }

        [Flags]
        public enum StateEnum : uint
        {
            MEM_COMMIT = 0x1000,
            MEM_FREE = 0x10000,
            MEM_RESERVE = 0x2000
        }

        [Flags]
        public enum TypeEnum : uint
        {
            MEM_IMAGE = 0x1000000,
            MEM_MAPPED = 0x40000,
            MEM_PRIVATE = 0x20000
        }
        #endregion

        #region Native structs
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public AllocationProtectEnum AllocationProtect;
            public IntPtr RegionSize;
            public StateEnum State;
            public AllocationProtectEnum Protect;
            public TypeEnum Type;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }

            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }
        #endregion

        #region Native functions
        [DllImport("kernel32.dll")]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        static extern int SetWindowLongPtr32(IntPtr hWnd, int nIndex, int value);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr value);

        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion
#endif
        public static void Init()
        {
#if WINDOWS
            ShowWindow(GetConsoleWindow(), 0);
#else
            throw new NotImplementedException();
#endif
        }

        public static VirtualMemoryInformation[] EnumerateVirtualMemorySpaces(Process proc)
        {
#if WINDOWS
            MEMORY_BASIC_INFORMATION m = new MEMORY_BASIC_INFORMATION();
            List<VirtualMemoryInformation> virtual_mem_entries = new List<VirtualMemoryInformation>();
            long address = 0;
            long maxAddress = (long)proc.MainModule.BaseAddress.ToInt64() > UInt32.MaxValue ? Int64.MaxValue : UInt32.MaxValue;
            VirtualMemoryPermissions perms = 0;
            do
            {
                VirtualQueryEx(proc.Handle, (IntPtr)address, out m, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
                perms = 0;
                if (m.Protect.HasFlag(AllocationProtectEnum.PAGE_READONLY))
                    perms = VirtualMemoryPermissions.READ;
                if (m.Protect.HasFlag(AllocationProtectEnum.PAGE_READWRITE))
                    perms = VirtualMemoryPermissions.READ | VirtualMemoryPermissions.WRITE;
                if (m.Protect.HasFlag(AllocationProtectEnum.PAGE_WRITECOMBINE) ||
                    m.Protect.HasFlag(AllocationProtectEnum.PAGE_WRITECOPY))
                    perms = VirtualMemoryPermissions.WRITE;
                if (m.Protect.HasFlag(AllocationProtectEnum.PAGE_EXECUTE))
                    perms = VirtualMemoryPermissions.EXECUTE;
                if (m.Protect.HasFlag(AllocationProtectEnum.PAGE_EXECUTE_READ))
                    perms = VirtualMemoryPermissions.EXECUTE | VirtualMemoryPermissions.READ;
                if (m.Protect.HasFlag(AllocationProtectEnum.PAGE_EXECUTE_WRITECOPY))
                    perms = VirtualMemoryPermissions.EXECUTE | VirtualMemoryPermissions.WRITE;
                if (m.Protect.HasFlag(AllocationProtectEnum.PAGE_EXECUTE_READWRITE))
                    perms = VirtualMemoryPermissions.EXECUTE | VirtualMemoryPermissions.READ | VirtualMemoryPermissions.WRITE;
                virtual_mem_entries.Add(new VirtualMemoryInformation(
                    m.BaseAddress.ToInt64(),
                    m.BaseAddress.ToInt64() + m.RegionSize.ToInt64(),
                    perms,
                    m.Type.HasFlag(TypeEnum.MEM_PRIVATE),
                    !m.Type.HasFlag(TypeEnum.MEM_PRIVATE)
                ));
                if (address == (long)m.BaseAddress + (long)m.RegionSize)
                    break;
                address = (long)m.BaseAddress + (long)m.RegionSize;
            } while (address < maxAddress);
            return virtual_mem_entries.ToArray();
#else
            throw new NotImplementedException();
#endif
        }

        public static byte[] ReadProcessMemory(Process proc, long address, int len)
        {
#if WINDOWS
            byte[] datas = new byte[len];
            ReadProcessMemory(proc.Handle, new IntPtr(address), datas, len, out IntPtr readBytesCount);
            return datas;
#else
            throw new NotImplementedException();
#endif
        }

        public static void WriteProcessMemory(Process proc, long address, byte[] datas)
        {
#if WINDOWS
            WriteProcessMemory(proc.Handle, new IntPtr(address), datas, datas.Length, out IntPtr writtenBytesCount);
#else
            throw new NotImplementedException();
#endif
        }

        public static IntPtr[] FindChildWindows(IntPtr window, ref List<IntPtr> windows)
        {
#if WINDOWS
            var procs = Process.GetProcesses();
            var proc = Array.Find(procs, p => p.MainWindowHandle == window);
            var handles = new List<IntPtr>();

            if (proc == null)
                return handles.ToArray();

            foreach (ProcessThread thread in proc.Threads)
                EnumThreadWindows(thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles.ToArray();
#else
            throw new NotImplementedException();
#endif
        }

        public static IntPtr FindWindow(string title)
        {
#if WINDOWS
            return FindWindow(IntPtr.Zero, title);
#else
            throw new NotImplementedException();
#endif
        }

        public static IntPtr FindWindowByPID(int id)
        {
#if WINDOWS
            Process proc = Process.GetProcessById(id);
            if (proc == null)
                return IntPtr.Zero;
            return proc.MainWindowHandle;
#else
            throw new NotImplementedException();
#endif
        }

        public static Point GetWindowPosition(IntPtr window)
        {
#if WINDOWS
            GetWindowRect(window, out RECT rect);
            return new Point(rect.X, rect.Y);
#else
            throw new NotImplementedException();
#endif
        }

        public static Size GetWindowSize(IntPtr window)
        {
#if WINDOWS
            GetWindowRect(window, out RECT rect);
            return new Size(rect.Width, rect.Height);
#else
            throw new NotImplementedException();
#endif
        }

        public static String GetWindowTitle(IntPtr window)
        {
#if WINDOWS
            StringBuilder windowText = new StringBuilder(512);
            GetWindowText(window, windowText, 512);
            return windowText.ToString();
#else
            throw new NotImplementedException();
#endif
        }

        public static void AttachWindow(IntPtr parent, IntPtr child)
        {
#if WINDOWS
            SetWindowLongPtr(child, GWL_STYLE, new IntPtr(GetWindowLongPtr(child, GWL_STYLE).ToInt32() | WS_CHILD));
            SetWindowLongPtr(child, GWL_EXSTYLE, new IntPtr(GetWindowLongPtr(child, GWL_EXSTYLE).ToInt32() | WS_EX_LAYERED | WS_EX_TRANSPARENT));
            SetParent(child, parent);
#else
            throw new NotImplementedException();
#endif
        }

#if WINDOWS
        static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr value)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, value);
            else
                return new IntPtr(SetWindowLongPtr32(hWnd, nIndex, value.ToInt32()));
        }
#endif
    }
}