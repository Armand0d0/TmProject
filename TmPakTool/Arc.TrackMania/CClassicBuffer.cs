using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    internal class CClassicBuffer : IDisposable
    {
        protected bool _writing;
        protected Stream _stream;
        private bool _disposed;

        public CClassicBuffer(Stream stream, bool write)
        {
            _stream = stream;
            _writing = write;
        }

        ~CClassicBuffer()
        {
            Dispose(false);
        }

        public Stream Stream
        {
            get { return _stream; }
        }

        public bool Writing
        {
            get { return _writing; }
        }

        public virtual uint Position
        {
            get { return (uint)_stream.Position; }
            set { _stream.Position = value; }
        }

        public virtual uint Size
        {
            get { return (uint)_stream.Length; }
        }

        public byte[] Bytes
        {
            get
            {
                if (!_stream.CanRead || !_stream.CanSeek)
                    return null;

                Position = 0;
                return ReadBytes(Size);
            }
        }

        public virtual void Initialize(byte[] data, uint offset, uint count)
        {

        }

        public virtual void FinishWriting()
        {

        }

        public byte[] ReadBytes(uint num)
        {
            byte[] result = new byte[num];
            Read(result, 0, (uint)result.Length);
            return result;
        }

        public virtual void Read(byte[] buff, uint offset, uint count)
        {
            _stream.Read(buff, (int)offset, (int)count);
        }

        public byte ReadByte()
        {
            return ReadBytes(1)[0];
        }

        public short ReadInt16()
        {
            return BitConverter.ToInt16(ReadBytes(2), 0);
        }

        public ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(ReadBytes(2), 0);
        }

        public int ReadInt32()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }

        public uint ReadUInt32()
        {
            return BitConverter.ToUInt32(ReadBytes(4), 0);
        }

        public long ReadInt64()
        {
            return BitConverter.ToInt64(ReadBytes(8), 0);
        }

        public ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(ReadBytes(8), 0);
        }

        public float ReadSingle()
        {
            return BitConverter.ToSingle(ReadBytes(4), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(ReadBytes(8), 0);
        }

        public string ReadString()
        {
            uint length = ReadUInt32();
            byte[] bytes = ReadBytes(length);
            return Encoding.ASCII.GetString(bytes);
        }

        public List<byte> ReadByteList()
        {
            int count = ReadInt32();
            List<byte> result = new List<byte>(count);
            for (int i = 0; i < count; i++)
                result.Add(ReadByte());
            return result;
        }

        public List<short> ReadInt16List()
        {
            int count = ReadInt32();
            List<short> result = new List<short>(count);
            for (int i = 0; i < count; i++)
                result.Add(ReadInt16());
            return result;
        }

        public List<ushort> ReadUInt16List()
        {
            int count = ReadInt32();
            List<ushort> result = new List<ushort>(count);
            for (int i = 0; i < count; i++)
                result.Add(ReadUInt16());
            return result;
        }

        public List<int> ReadInt32List()
        {
            int count = ReadInt32();
            List<int> result = new List<int>(count);
            for (int i = 0; i < count; i++)
                result.Add(ReadInt32());
            return result;
        }

        public List<uint> ReadUInt32List()
        {
            int count = ReadInt32();
            List<uint> result = new List<uint>(count);
            for (int i = 0; i < count; i++)
                result.Add(ReadUInt32());
            return result;
        }

        public List<long> ReadInt64List()
        {
            int count = ReadInt32();
            List<long> result = new List<long>(count);
            for (int i = 0; i < count; i++)
                result.Add(ReadInt64());
            return result;
        }

        public List<ulong> ReadUInt64List()
        {
            int count = ReadInt32();
            List<ulong> result = new List<ulong>(count);
            for (int i = 0; i < count; i++)
                result.Add(ReadUInt64());
            return result;
        }

        public List<float> ReadSingleList()
        {
            int count = ReadInt32();
            List<float> result = new List<float>(count);
            for (int i = 0; i < count; i++)
                result.Add(ReadSingle());
            return result;
        }

        public List<double> ReadDoubleList()
        {
            int count = ReadInt32();
            List<double> result = new List<double>(count);
            for (int i = 0; i < count; i++)
                result.Add(ReadDouble());
            return result;
        }

        public List<string> ReadStringList()
        {
            int count = ReadInt32();
            List<string> result = new List<string>(count);
            for (int i = 0; i < count; i++)
                result.Add(ReadString());
            return result;
        }

        public void Write(byte[] buf)
        {
            Write(buf, 0, (uint)buf.Length);
        }

        public virtual void Write(byte[] buf, uint offset, uint count)
        {
            _stream.Write(buf, (int)offset, (int)count);
        }

        public void Write(byte b)
        {
            Write(new byte[] { b });
        }

        public void Write(short s)
        {
            Write(BitConverter.GetBytes(s));
        }

        public void Write(ushort us)
        {
            Write(BitConverter.GetBytes(us));
        }

        public void Write(int i)
        {
            Write(BitConverter.GetBytes(i));
        }

        public void Write(uint ui)
        {
            Write(BitConverter.GetBytes(ui));
        }

        public void Write(long l)
        {
            Write(BitConverter.GetBytes(l));
        }

        public void Write(ulong ul)
        {
            Write(BitConverter.GetBytes(ul));
        }

        public void Write(float f)
        {
            Write(BitConverter.GetBytes(f));
        }

        public void Write(double d)
        {
            Write(BitConverter.GetBytes(d));
        }

        public void Write(string s)
        {
            byte[] bytes;
            if (s == null)
                bytes = new byte[0];
            else
                bytes = Encoding.ASCII.GetBytes(s);
            Write(bytes.Length);
            Write(bytes);
        }

        public void Write(List<byte> lb)
        {
            Write(lb.Count);
            foreach (byte b in lb)
                Write(b);
        }

        public void Write(List<short> ls)
        {
            Write(ls.Count);
            foreach (short s in ls)
                Write(s);
        }

        public void Write(List<ushort> lus)
        {
            Write(lus.Count);
            foreach (ushort us in lus)
                Write(us);
        }

        public void Write(List<int> li)
        {
            Write(li.Count);
            foreach (int i in li)
                Write(i);
        }

        public void Write(List<uint> lui)
        {
            Write(lui.Count);
            foreach (uint ui in lui)
                Write(ui);
        }

        public void Write(List<long> ll)
        {
            Write(ll.Count);
            foreach (long l in ll)
                Write(l);
        }

        public void Write(List<ulong> lul)
        {
            Write(lul.Count);
            foreach (ulong ul in lul)
                Write(ul);
        }

        public void Write(List<float> lf)
        {
            Write(lf.Count);
            foreach (float f in lf)
                Write(f);
        }

        public void Write(List<double> ld)
        {
            Write(ld.Count);
            foreach (double d in ld)
                Write(d);
        }

        public void Write(List<string> ls)
        {
            Write(ls.Count);
            foreach (string s in ls)
                Write(s);
        }

        public void ReadWrite(byte[] buf)
        {
            if (_writing)
                Write(buf, 0, (uint)buf.Length);
            else
                Read(buf, 0, (uint)buf.Length);
        }

        public void ReadWrite(ref byte b)
        {
            if (_writing)
                Write(b);
            else
                b = ReadByte();
        }

        public void ReadWrite(ref short s)
        {
            if (_writing)
                Write(s);
            else
                s = ReadInt16();
        }

        public void ReadWrite(ref ushort us)
        {
            if (_writing)
                Write(us);
            else
                us = ReadUInt16();
        }

        public void ReadWrite(ref int i)
        {
            if (_writing)
                Write(i);
            else
                i = ReadInt32();
        }

        public void ReadWrite(ref uint ui)
        {
            if (_writing)
                Write(ui);
            else
                ui = ReadUInt32();
        }

        public void ReadWrite(ref long l)
        {
            if (_writing)
                Write(l);
            else
                l = ReadInt64();
        }

        public void ReadWrite(ref ulong ul)
        {
            if (_writing)
                Write(ul);
            else
                ul = ReadUInt64();
        }

        public void ReadWrite(ref float f)
        {
            if (_writing)
                Write(f);
            else
                f = ReadSingle();
        }

        public void ReadWrite(ref double d)
        {
            if (_writing)
                Write(d);
            else
                d = ReadDouble();
        }

        public void ReadWrite(string literal)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(literal);
            if (_writing)
            {
                Write(bytes);
            }
            else
            {
                string read = Encoding.ASCII.GetString(ReadBytes((uint)literal.Length));
                if (read != literal)
                    throw new Exception(string.Format("Expected literal {0}, got {1} instead", literal, read));
            }
        }

        public void ReadWrite(ref string s)
        {
            if (_writing)
                Write(s);
            else
                s = ReadString();
        }

        public void ReadWrite(ref List<byte> lb)
        {
            if (_writing)
                Write(lb);
            else
                lb = ReadByteList();
        }

        public void ReadWrite(ref List<short> ls)
        {
            if (_writing)
                Write(ls);
            else
                ls = ReadInt16List();
        }

        public void ReadWrite(ref List<ushort> lus)
        {
            if (_writing)
                Write(lus);
            else
                lus = ReadUInt16List();
        }

        public void ReadWrite(ref List<int> li)
        {
            if (_writing)
                Write(li);
            else
                li = ReadInt32List();
        }

        public void ReadWrite(ref List<uint> lui)
        {
            if (_writing)
                Write(lui);
            else
                lui = ReadUInt32List();
        }

        public void ReadWrite(ref List<long> ll)
        {
            if (_writing)
                Write(ll);
            else
                ll = ReadInt64List();
        }

        public void ReadWrite(ref List<ulong> lul)
        {
            if (_writing)
                Write(lul);
            else
                lul = ReadUInt64List();
        }

        public void ReadWrite(ref List<float> lf)
        {
            if (_writing)
                Write(lf);
            else
                lf = ReadSingleList();
        }

        public void ReadWrite(ref List<double> ld)
        {
            if (_writing)
                Write(ld);
            else
                ld = ReadDoubleList();
        }

        public void ReadWrite(ref List<string> ls)
        {
            if (_writing)
                Write(ls);
            else
                ls = ReadStringList();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _stream.Dispose();
                    _stream = null;
                }
                _disposed = true;
            }
        }
    }
}
