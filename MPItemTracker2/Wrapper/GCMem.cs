using System;
using System.Diagnostics;
using System.Linq;
using Imports;

namespace Wrapper
{
    public static class GCMem
    {
        static Process dolphin = null;
        static readonly long GCRAMBaseAddr = 0x80000000;
        static long RAMBaseAddr = 0;
        static bool initialized = false;

        internal static Byte[] Read(long gc_address, int size)
        {
            long pc_address = 0;
            if (size == 0)
                return new byte[0];
            try {
                pc_address = RAMBaseAddr + (gc_address - GCRAMBaseAddr);
                return ImportsMgr.ReadProcessMemory(dolphin, pc_address, size);
            } catch { 
                return null;
            }
        }

        internal static void Write(long gc_address, Byte[] datas)
        {
            long pc_address = 0;
            if (datas == null)
                return;
            try {
                pc_address = RAMBaseAddr + (gc_address - GCRAMBaseAddr);
                ImportsMgr.WriteProcessMemory(dolphin, pc_address, datas);
            } catch { }
        }

        internal static Byte ReadUInt8(long gc_address)
        {
            return Read(gc_address, 1)[0];
        }

        internal static UInt16 ReadUInt16(long gc_address)
        {
            return BitConverter.ToUInt16(Read(gc_address, 2).Reverse().ToArray(), 0);
        }

        internal static UInt32 ReadUInt32(long gc_address)
        {
            return BitConverter.ToUInt32(Read(gc_address, 4).Reverse().ToArray(), 0);
        }

        internal static UInt64 ReadUInt64(long gc_address)
        {
            return BitConverter.ToUInt64(Read(gc_address, 8).Reverse().ToArray(), 0);
        }

        internal static SByte ReadInt8(long gc_address)
        {
            return (SByte)Read(gc_address, 1)[0];
        }

        internal static Int16 ReadInt16(long gc_address)
        {
            return BitConverter.ToInt16(Read(gc_address, 2).Reverse().ToArray(), 0);
        }

        internal static Int32 ReadInt32(long gc_address)
        {
            return BitConverter.ToInt32(Read(gc_address, 4).Reverse().ToArray(), 0);
        }

        internal static Int64 ReadInt64(long gc_address)
        {
            return BitConverter.ToInt64(Read(gc_address, 8).Reverse().ToArray(), 0);
        }

        internal static Single ReadFloat32(long gc_address)
        {
            return BitConverter.ToSingle(Read(gc_address, 4).Reverse().ToArray(), 0);
        }

        internal static Double ReadFloat64(long gc_address)
        {
            return BitConverter.ToDouble(Read(gc_address, 8).Reverse().ToArray(), 0);
        }

        internal static void WriteUInt8(long gc_address, Byte value)
        {
            Write(gc_address, new Byte[] { value });
        }

        internal static void WriteUInt16(long gc_address, UInt16 value)
        {
            Write(gc_address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        internal static void WriteUInt32(long gc_address, UInt32 value)
        {
            Write(gc_address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        internal static void WriteUInt64(long gc_address, UInt64 value)
        {
            Write(gc_address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        internal static void WriteInt8(long gc_address, SByte value)
        {
            Write(gc_address, new Byte[] { (Byte)value });
        }

        internal static void WriteInt16(long gc_address, Int16 value)
        {
            Write(gc_address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        internal static void WriteInt32(long gc_address, Int32 value)
        {
            Write(gc_address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        internal static void WriteInt64(long gc_address, Int64 value)
        {
            Write(gc_address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        internal static void WriteFloat32(long gc_address, Single value)
        {
            Write(gc_address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        internal static void WriteFloat64(long gc_address, Double value)
        {
            Write(gc_address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        internal static void Init(Process proc, long ram_baseaddr)
        {
            if (!initialized)
            {
                dolphin = proc;
                if (dolphin == null)
                    return;
                RAMBaseAddr = ram_baseaddr;
                initialized = true;
            }
        }

        internal static void DeInit()
        {
            dolphin = null;
            RAMBaseAddr = 0;
            initialized = false;
        }
    }
}
