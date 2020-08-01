using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace Prime.Memory
{
    class Utils
    {
        #region C imports
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

        public enum StateEnum : uint
        {
            MEM_COMMIT = 0x1000,
            MEM_FREE = 0x10000,
            MEM_RESERVE = 0x2000
        }

        public enum TypeEnum : uint
        {
            MEM_IMAGE = 0x1000000,
            MEM_MAPPED = 0x40000,
            MEM_PRIVATE = 0x20000
        }

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
        #endregion

        internal static Byte[] Read(Process proc, long address, int size)
        {
            if (proc.HasExited)
                return null;
            if (size == 0)
                return new byte[0];
            byte[] datas = new byte[size];
            IntPtr readBytesCount = IntPtr.Zero;
            ReadProcessMemory(proc.Handle, new IntPtr(address), datas, size, out readBytesCount);
            return datas;
        }

        internal static Byte ReadUInt8(Process proc, long address)
        {
            return Read(proc, address, 1)[0];
        }

        internal static UInt16 ReadUInt16(Process proc, long address)
        {
            return BitConverter.ToUInt16(Read(proc, address, 2), 0);
        }

        internal static UInt32 ReadUInt32(Process proc, long address)
        {
            return BitConverter.ToUInt32(Read(proc, address, 4), 0);
        }

        internal static UInt64 ReadUInt64(Process proc, long address)
        {
            return BitConverter.ToUInt64(Read(proc, address, 8), 0);
        }

        internal static SByte ReadInt8(Process proc, long address)
        {
            return (SByte)Read(proc, address, 1)[0];
        }

        internal static Int16 ReadInt16(Process proc, long address)
        {
            return BitConverter.ToInt16(Read(proc, address, 2), 0);
        }

        internal static Int32 ReadInt32(Process proc, long address)
        {
            return BitConverter.ToInt32(Read(proc, address, 4), 0);
        }

        internal static Int64 ReadInt64(Process proc, long address)
        {
            return BitConverter.ToInt64(Read(proc, address, 8), 0);
        }

        internal static Single ReadFloat32(Process proc, long address)
        {
            return BitConverter.ToSingle(Read(proc, address, 4), 0);
        }

        internal static Double ReadFloat64(Process proc, long address)
        {
            return BitConverter.ToDouble(Read(proc, address, 8), 0);
        }

        internal static void Write(Process proc, long address, Byte[] datas)
        {
            if (proc.HasExited)
                return;
            if (datas == null)
                return;
            IntPtr writtenBytesCount = IntPtr.Zero;
            WriteProcessMemory(proc.Handle, new IntPtr(address), datas, datas.Length, out writtenBytesCount);
        }

        internal static void WriteUInt8(Process proc, long address, Byte value)
        {
            Write(proc, address, new Byte[] { value });
        }

        internal static void WriteUInt16(Process proc, long address, UInt16 value)
        {
            Write(proc, address, BitConverter.GetBytes(value));
        }

        internal static void WriteUInt32(Process proc, long address, UInt32 value)
        {
            Write(proc, address, BitConverter.GetBytes(value));
        }

        internal static void WriteUInt64(Process proc, long address, UInt64 value)
        {
            Write(proc, address, BitConverter.GetBytes(value));
        }

        internal static void WriteInt8(Process proc, long address, SByte value)
        {
            Write(proc, address, new Byte[] { (Byte)value });
        }

        internal static void WriteInt16(Process proc, long address, Int16 value)
        {
            Write(proc, address, BitConverter.GetBytes(value));
        }

        internal static void WriteInt32(Process proc, long address, Int32 value)
        {
            Write(proc, address, BitConverter.GetBytes(value));
        }

        internal static void WriteInt64(Process proc, long address, Int64 value)
        {
            Write(proc, address, BitConverter.GetBytes(value));
        }

        internal static void WriteFloat32(Process proc, long address, Single value)
        {
            Write(proc, address, BitConverter.GetBytes(value));
        }

        internal static void WriteFloat64(Process proc, long address, Double value)
        {
            Write(proc, address, BitConverter.GetBytes(value));
        }

        internal static MEMORY_BASIC_INFORMATION CS_VirtualQuery(Process proc, long address)
        {
            MEMORY_BASIC_INFORMATION m = new MEMORY_BASIC_INFORMATION();
            VirtualQueryEx(proc.Handle, (IntPtr)address, out m, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
            return m;
        }
    }
}
