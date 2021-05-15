using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
#if WINDOWS
using _Imports = Imports.Windows;
#elif LINUX
using _Imports = Imports.Linux;
#endif

namespace Imports
{
    [Flags]
    public enum VirtualMemoryPermissions
    {
        READ = 1,
        WRITE = 2,
        EXECUTE = 4
    }

    public struct VirtualMemoryInformation
    {
        public readonly long BaseAddress;
        public readonly long Size;
        public readonly VirtualMemoryPermissions Permissions;
        public readonly bool IsPrivate;
        public readonly bool IsShared;

        public VirtualMemoryInformation(long start_addr, long end_addr, VirtualMemoryPermissions perms, bool is_private, bool is_shared)
        {
            BaseAddress = start_addr;
            Size = end_addr - start_addr;
            Permissions = perms;
            IsPrivate = is_private;
            IsShared = is_shared;
        }
    }

    public abstract class ImportsMgr
    {
        public static void Init()
        {
            _Imports.Init();
        }
        public static void DeInit()
        {
#if LINUX
            _Imports.DeInit();
#endif
        }
        public static VirtualMemoryInformation[] EnumerateVirtualMemorySpaces(Process proc)
        {
            return _Imports.EnumerateVirtualMemorySpaces(proc);
        }
        public static byte[] ReadProcessMemory(Process proc, long address, int len)
        {
            return _Imports.ReadProcessMemory(proc, address, len);
        }
        public static void WriteProcessMemory(Process dolphin, long pc_address, byte[] datas)
        {
            _Imports.WriteProcessMemory(dolphin, pc_address, datas);
        }
        public static IntPtr[] FindChildWindows(IntPtr window, ref List<IntPtr> childWindows)
        {
            return _Imports.FindChildWindows(window, ref childWindows);
        }
        public static IntPtr FindWindow(string title)
        {
            return _Imports.FindWindow(title);
        }
        public static IntPtr FindWindowByPID(int pid)
        {
            return _Imports.FindWindowByPID(pid);
        }
        public static Point GetWindowPosition(IntPtr window)
        {
            return _Imports.GetWindowPosition(window);
        }
        public static Size GetWindowSize(IntPtr window)
        {
            return _Imports.GetWindowSize(window);
        }
        public static String GetWindowTitle(IntPtr window)
        {
            return _Imports.GetWindowTitle(window);
        }
        public static void AttachWindow(IntPtr parent, IntPtr child)
        {
            _Imports.AttachWindow(parent, child);
        }
    }
}
