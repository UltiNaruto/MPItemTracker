using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Prime.Memory
{
    public class MemStream : Stream
    {
        private List<byte> datas;
        private long pos;
        public override long Length { get { return this.datas.Count; } }
        public override long Position { get { return this.pos; } set { this.pos = value; } }
        public override bool CanRead { get { return true; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return true; } }

        public MemStream(byte[] p)
        {
            this.datas = p.ToList();
            this.pos = 0;
        }

        public MemStream(byte[] p, int offset, int len) : this(p)
        {
            for (int i = 0; i < len && p.Length > offset + 1; i++)
                this.datas.RemoveAt(offset);
        }

        public MemStream()
        {
            this.datas = new List<byte>();
            this.pos = 0;
        }

        public static MemStream operator +(MemStream a, byte b)
        {
            for (int i = 0; i < a.datas.Count; i++)
                a.datas[i] += b;
            return a;
        }

        public static MemStream operator ^(MemStream a, byte b)
        {
            for (int i = 0; i < a.datas.Count; i++)
                a.datas[i] ^= b;
            return a;
        }

        public static MemStream operator -(MemStream a, byte b)
        {
            for (int i = 0; i < a.datas.Count; i++)
                a.datas[i] += b;
            return a;
        }

        public static MemStream operator *(MemStream a, byte b)
        {
            for (int i = 0; i < a.datas.Count; i++)
                a.datas[i] *= b;
            return a;
        }

        public static MemStream operator /(MemStream a, byte b)
        {
            for (int i = 0; i < a.datas.Count; i++)
                a.datas[i] /= b;
            return a;
        }

        public static MemStream operator %(MemStream a, byte b)
        {
            for (int i = 0; i < a.datas.Count; i++)
                a.datas[i] %= b;
            return a;
        }

        public byte[] GetBuffer()
        {
            return this.datas.ToArray();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return Read(ref buffer, offset, count);
        }

        public int Read(ref byte[] buffer, int offset, int count)
        {
            this.Seek(offset, SeekOrigin.Current);
            if (this.pos + count > this.Length)
            {
                return 0;
            }
            else
            {
                List<byte> buf = new List<byte>();
                for (int i = 0; i < count; i++)
                {
                    buf.Add(this.datas[(int)this.pos]);
                    this.Seek(1, SeekOrigin.Current);
                }
                buffer = new byte[buf.Count];
                for (int i = 0; i < buf.Count; i++)
                    buffer[i] = buf[i];
                return buf.Count;
            }
        }

        public override int ReadByte()
        {
            return this.ReadUInt8();
        }

        public Byte ReadUInt8()
        {
            byte[] buf = new byte[1];
            this.Read(ref buf, 0, buf.Length);
            return buf[0];
        }

        public SByte ReadInt8()
        {
            byte[] buf = new byte[sizeof(SByte)];
            this.Read(ref buf, 0, buf.Length);
            return (SByte)buf[0];
        }

        public bool ReadBoolean()
        {
            byte[] buf = new byte[1];
            this.Read(ref buf, 0, buf.Length);
            return BitConverter.ToBoolean(buf, 0);
        }

        public byte[] ReadBytes(int len)
        {
            byte[] buf = new byte[len];
            this.Read(ref buf, 0, buf.Length);
            return buf;
        }

        public UInt16 ReadUInt16()
        {
            byte[] buf = new byte[sizeof(UInt16)];
            this.Read(ref buf, 0, buf.Length);
            return BitConverter.ToUInt16(buf, 0);
        }

        public Int16 ReadInt16()
        {
            byte[] buf = new byte[sizeof(Int16)];
            this.Read(ref buf, 0, buf.Length);
            return BitConverter.ToInt16(buf, 0);
        }

        public UInt32 ReadUInt32()
        {
            byte[] buf = new byte[sizeof(UInt32)];
            this.Read(ref buf, 0, buf.Length);
            return BitConverter.ToUInt32(buf, 0);
        }

        public Int32 ReadInt32()
        {
            byte[] buf = new byte[sizeof(Int32)];
            this.Read(ref buf, 0, buf.Length);
            return BitConverter.ToInt32(buf, 0);
        }

        public Single ReadFloat()
        {
            byte[] buf = new byte[sizeof(Single)];
            this.Read(ref buf, 0, buf.Length);
            return BitConverter.ToSingle(buf, 0);
        }

        public Double ReadDouble()
        {
            byte[] buf = new byte[sizeof(Double)];
            this.Read(ref buf, 0, buf.Length);
            return BitConverter.ToDouble(buf, 0);
        }

        public String ReadString(int len)
        {
            byte[] buf = new byte[len];
            this.Read(ref buf, 0, buf.Length);
            return Encoding.GetEncoding("euc-kr").GetString(buf);
        }

        public String ReadWString(int len)
        {
            byte[] buf = new byte[len * 2];
            this.Read(ref buf, 0, buf.Length);
            return Encoding.Unicode.GetString(buf).TrimEnd('\0');
        }

        public override void Write(byte[] d, int p, int p_2)
        {
            for (int i = 0; i < p_2; i++)
            {
                if (p > 0)
                    this.datas.Insert(p + i, d[i]);
                else
                {
                    this.datas.Add(d[i]);
                    this.Seek(1, SeekOrigin.Current);
                }
            }
        }

        public void Write(byte[] v)
        {
            this.Write(v, 0, v.Length);
        }

        public void Write(bool v)
        {
            this.Write(BitConverter.GetBytes(v), 0, sizeof(bool));
        }

        public void Write(byte v)
        {
            this.Write(BitConverter.GetBytes(v), 0, sizeof(byte));
        }

        public void Write(sbyte v)
        {
            this.Write(BitConverter.GetBytes(v), 0, sizeof(sbyte));
        }

        public void Write(short v)
        {
            this.Write(BitConverter.GetBytes(v), 0, sizeof(short));
        }

        public void Write(ushort v)
        {
            this.Write(BitConverter.GetBytes(v), 0, sizeof(ushort));
        }

        public void Write(int v)
        {
            this.Write(BitConverter.GetBytes(v), 0, sizeof(int));
        }

        public void Write(uint v)
        {
            this.Write(BitConverter.GetBytes(v), 0, sizeof(uint));
        }

        public void Write(long v)
        {
            this.Write(BitConverter.GetBytes(v), 0, sizeof(long));
        }

        public void Write(char v)
        {
            this.Write(BitConverter.GetBytes(v), 0, sizeof(char));
        }

        public void Write(float v)
        {
            this.Write(BitConverter.GetBytes(v), 0, sizeof(float));
        }

        public void Write(double v)
        {
            this.Write(BitConverter.GetBytes(v), 0, sizeof(double));
        }

        public void Replace(int offset, SByte value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, Int16 value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, Int32 value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, Int64 value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, Byte value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, UInt16 value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, UInt32 value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, UInt64 value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, Single value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, Double value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, Char value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, Boolean value)
        {
            Replace(offset, BitConverter.GetBytes(value));
        }

        public void Replace(int offset, String value, Encoding enc = null)
        {
            if (enc == null)
                Replace(offset, Encoding.Default.GetBytes(value));
            else
                Replace(offset, enc.GetBytes(value));
        }

        public void Replace(int offset, byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
                this.datas[offset + i] = value[i];
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
                this.pos = offset;
            else if (origin == SeekOrigin.Current)
                this.pos += offset;
            else if (origin == SeekOrigin.End)
            {
                if (offset > 0)
                    throw new Exception("Offset cannot be positive while using SeekOrigin.End");
                if (-offset > this.pos)
                    throw new IndexOutOfRangeException();
                this.pos -= offset;
            }

            return this.pos;
        }

        public override void SetLength(long value)
        {
            for (int i = 0; i < value; i++)
            {
                if (value < this.datas.Count)
                    this.datas.RemoveAt(this.datas.Count - 1);
                else if (value > this.datas.Count)
                    this.datas.Add(0);
            }
        }

        internal byte[] ToArray()
        {
            return this.datas.ToArray();
        }
    }
}