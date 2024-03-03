using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes.MwFoundations;

namespace Arc.TrackMania
{
    internal class CClassicArchive
    {
        private Stack<CClassicBuffer> _buffers = new Stack<CClassicBuffer>();
        private CMwNod _mainNode;
        private List<CMwNod> _nodes;
        private List<string> _knownStrings;

        public CClassicArchive()
        {
            
        }

        public CClassicArchive(CClassicBuffer buffer)
        {
            _buffers.Push(buffer);
        }

        public void Init(CMwNod mainNode, int numNodes)
        {
            _mainNode = mainNode;
            _nodes = new List<CMwNod>();
            for (int i = 0; i < numNodes; i++)
                _nodes.Add(null);

            _nodes[0] = mainNode;
        }

        public void ResetNodesWritten()
        {
            _nodes = new List<CMwNod>();
            _nodes.Add(MainNode);
        }

        public void ResetKnownStrings()
        {
            _knownStrings = null;
        }

        public CClassicBuffer Buffer
        {
            get
            {
                if (_buffers.Count > 0)
                    return _buffers.Peek();
                else
                    return null;
            }
        }

        public bool Writing
        {
            get { return Buffer.Writing; }
        }

        public uint Position
        {
            get { return Buffer.Position; }
            set { Buffer.Position = value; }
        }

        public uint Size
        {
            get { return Buffer.Size; }
        }

        public byte[] ReadBytes(uint num)
        {
            return Buffer.ReadBytes(num);
        }

        public byte ReadByte()
        {
            return Buffer.ReadByte();
        }

        public short ReadInt16()
        {
            return Buffer.ReadInt16();
        }

        public ushort ReadUInt16()
        {
            return Buffer.ReadUInt16();
        }

        public int ReadInt32()
        {
            return Buffer.ReadInt32();
        }

        public uint ReadUInt32()
        {
            return Buffer.ReadUInt32();
        }

        public long ReadInt64()
        {
            return Buffer.ReadInt64();
        }

        public ulong ReadUInt64()
        {
            return Buffer.ReadUInt64();
        }

        public float ReadSingle()
        {
            return Buffer.ReadSingle();
        }

        public double ReadDouble()
        {
            return Buffer.ReadDouble();
        }

        public string ReadString()
        {
            return Buffer.ReadString();
        }

        public string ReadLookbackString()
        {
            InitLookbackStrings(false);

            string s;
            int index = Buffer.ReadInt32();
            if (index == -1)
            {
                s = null;
            }
            else if ((index & 0x3FFFFFFF) == 0)
            {
                s = ReadString();
                _knownStrings.Add(s);
            }
            else
            {
                s = _knownStrings[(index & 0x3FFFFFFF) - 1];
            }
            return s;
        }

        public List<byte> ReadByteList()
        {
            return Buffer.ReadByteList();
        }

        public List<short> ReadInt16List()
        {
            return Buffer.ReadInt16List();
        }

        public List<ushort> ReadUInt16List()
        {
            return Buffer.ReadUInt16List();
        }

        public List<int> ReadInt32List()
        {
            return Buffer.ReadInt32List();
        }

        public List<uint> ReadUInt32List()
        {
            return Buffer.ReadUInt32List();
        }

        public List<long> ReadInt64List()
        {
            return Buffer.ReadInt64List();
        }

        public List<ulong> ReadUInt64List()
        {
            return Buffer.ReadUInt64List();
        }

        public List<float> ReadSingleList()
        {
            return Buffer.ReadSingleList();
        }

        public List<double> ReadDoubleList()
        {
            return Buffer.ReadDoubleList();
        }

        public List<string> ReadStringList()
        {
            return Buffer.ReadStringList();
        }

        public List<string> ReadLookbackStringList()
        {
            int count = ReadInt32();
            List<string> result = new List<string>(count);
            for (int i = 0; i < count; i++)
                result.Add(ReadLookbackString());
            return result;
        }

        public void Write(byte[] buf)
        {
            Buffer.Write(buf);
        }

        public void Write(byte b)
        {
            Buffer.Write(b);
        }

        public void Write(short s)
        {
            Buffer.Write(s);
        }

        public void Write(ushort us)
        {
            Buffer.Write(us);
        }

        public void Write(int i)
        {
            Buffer.Write(i);
        }

        public void Write(uint ui)
        {
            Buffer.Write(ui);
        }

        public void Write(long l)
        {
            Buffer.Write(l);
        }

        public void Write(ulong ul)
        {
            Buffer.Write(ul);
        }

        public void Write(float f)
        {
            Buffer.Write(f);
        }

        public void Write(double d)
        {
            Buffer.Write(d);
        }

        public void Write(string s)
        {
            Buffer.Write(s);
        }

        public void WriteLookbackString(string s)
        {
            InitLookbackStrings(true);

            if (s == null)
            {
                Write(-1);
                return;
            }

            int index = _knownStrings.IndexOf(s);
            if (index < 0)
            {
                _knownStrings.Add(s);
                Write(0x40000000);
                Write(s);
            }
            else
            {
                Write(0x40000000 | (uint)(index + 1));
            }
        }

        public void Write(List<byte> lb)
        {
            Buffer.Write(lb);
        }

        public void Write(List<short> ls)
        {
            Buffer.Write(ls);
        }

        public void Write(List<ushort> lus)
        {
            Buffer.Write(lus);
        }

        public void Write(List<int> li)
        {
            Buffer.Write(li);
        }

        public void Write(List<uint> lui)
        {
            Buffer.Write(lui);
        }

        public void Write(List<long> ll)
        {
            Buffer.Write(ll);
        }

        public void Write(List<ulong> lul)
        {
            Buffer.Write(lul);
        }

        public void Write(List<float> lf)
        {
            Buffer.Write(lf);
        }

        public void Write(List<double> ld)
        {
            Buffer.Write(ld);
        }

        public void Write(List<string> ls)
        {
            Buffer.Write(ls);
        }

        public void WriteLookbackStringList(List<string> ls)
        {
            Write(ls.Count);
            foreach (string s in ls)
                WriteLookbackString(s);
        }

        public void ReadWrite(byte[] buf)
        {
            Buffer.ReadWrite(buf);
        }

        public void ReadWrite(ref byte b)
        {
            Buffer.ReadWrite(ref b);
        }

        public void ReadWrite(ref short s)
        {
            Buffer.ReadWrite(ref s);
        }

        public void ReadWrite(ref ushort us)
        {
            Buffer.ReadWrite(ref us);
        }

        public void ReadWrite(ref int i)
        {
            Buffer.ReadWrite(ref i);
        }

        public void ReadWrite(ref uint ui)
        {
            Buffer.ReadWrite(ref ui);
        }

        public void ReadWrite(ref long l)
        {
            Buffer.ReadWrite(ref l);
        }

        public void ReadWrite(ref ulong ul)
        {
            Buffer.ReadWrite(ref ul);
        }

        public void ReadWrite(ref float f)
        {
            Buffer.ReadWrite(ref f);
        }

        public void ReadWrite(ref double d)
        {
            Buffer.ReadWrite(ref d);
        }

        public void ReadWrite(string literal)
        {
            Buffer.ReadWrite(literal);
        }

        public void ReadWrite(ref string s)
        {
            Buffer.ReadWrite(ref s);
        }

        public void ReadWriteLookbackString(ref string s)
        {
            if (Writing)
                WriteLookbackString(s);
            else
                s = ReadLookbackString();
        }

        public void ReadWrite<T>(ref T item) where T : IReadWrite, new()
        {
            if (!Writing)
                item = new T();

            item.ReadWrite(this);
        }

        public void ReadWrite<T>(ref T item, int version) where T : IReadWriteEx, new()
        {
            if (!Writing)
                item = new T();

            item.ReadWrite(this, version);
        }

        public void ReadWriteNode<T>(ref T node) where T : CMwNod
        {
            if (Writing)
            {
                if (node == null)
                {
                    Write(-1);
                    return;
                }

                int index = _nodes.IndexOf(node);
                if (index < 0)
                {
                    index = _nodes.Count;
                    _nodes.Add(node);

                    Write(index);
                    Write(node.ID);
                    node.ReadWrite(this);
                }
                else
                {
                    Write(index);
                }
            }
            else
            {
                int index = ReadInt32();
                if (index < 0)
                {
                    node = null;
                    return;
                }

                if (_nodes[index] == null)
                {
                    uint classID = ReadUInt32();
                    node = CMwEngineManager.CreateClassInstance(classID) as T;
                    if (node == null)
                    {
                        throw new Exception(string.Format("Class ID {0:X08} ({1}) is not supported",
                            classID, CMwEngineManager.GetClassName(classID)));
                    }

                    node.ReadWrite(this);
                    _nodes[index] = node;
                }
                else
                {
                    node = _nodes[index] as T;
                }
            }
        }

        public void ReadWrite(ref List<byte> lb)
        {
            Buffer.ReadWrite(ref lb);
        }

        public void ReadWrite(ref List<short> ls)
        {
            Buffer.ReadWrite(ref ls);
        }

        public void ReadWrite(ref List<ushort> lus)
        {
            Buffer.ReadWrite(ref lus);
        }

        public void ReadWrite(ref List<int> li)
        {
            Buffer.ReadWrite(ref li);
        }

        public void ReadWrite(ref List<uint> lui)
        {
            Buffer.ReadWrite(ref lui);
        }

        public void ReadWrite(ref List<long> ll)
        {
            Buffer.ReadWrite(ref ll);
        }

        public void ReadWrite(ref List<ulong> lul)
        {
            Buffer.ReadWrite(ref lul);
        }

        public void ReadWrite(ref List<float> lf)
        {
            Buffer.ReadWrite(ref lf);
        }

        public void ReadWrite(ref List<double> ld)
        {
            Buffer.ReadWrite(ref ld);
        }

        public void ReadWrite(ref List<string> ls)
        {
            Buffer.ReadWrite(ref ls);
        }

        public void ReadWriteLookbackStringList(ref List<string> ls)
        {
            if (Writing)
                WriteLookbackStringList(ls);
            else
                ls = ReadLookbackStringList();
        }

        public void ReadWriteList<T>(ref List<T> list) where T : IReadWrite, new()
        {
            if (Writing)
            {
                Write(list.Count);
                foreach (T item in list)
                    item.ReadWrite(this);
            }
            else
            {
                int count = ReadInt32();
                list = new List<T>(count);
                for (int i = 0; i < count; i++)
                {
                    T item = new T();
                    item.ReadWrite(this);
                    list.Add(item);
                }
            }
        }

        public void ReadWriteNodeList<T>(ref List<T> ln) where T : CMwNod
        {
            if (Writing)
            {
                Write(ln.Count);
                for (int i = 0; i < ln.Count; i++)
                {
                    T node = ln[i];
                    ReadWriteNode<T>(ref node);
                }
            }
            else
            {
                int count = ReadInt32();
                ln = new List<T>(count);
                for (int i = 0; i < count; i++)
                {
                    T node = null;
                    ReadWriteNode<T>(ref node);
                    ln.Add(node);
                }
            }
        }

        public void PushBuffer(bool write)
        {
            _buffers.Push(new CClassicBufferMemory(write));
        }

        public void PushBuffer(byte[] data)
        {
            _buffers.Push(new CClassicBufferMemory(data, false));
        }

        public void PushBuffer(CClassicBuffer buffer)
        {
            _buffers.Push(buffer);
        }

        public byte[] PopBuffer()
        {
            return _buffers.Pop().Bytes;
        }

        public CMwNod MainNode
        {
            get { return _mainNode; }
            set { _mainNode = value; }
        }

        public List<CMwNod> Nodes
        {
            get { return _nodes; }
        }

        private void InitLookbackStrings(bool writing)
        {
            if (_knownStrings != null)
                return;

            _knownStrings = new List<string>();
            int version = 3;
            if (writing)
            {
                Write(version);
            }
            else
            {
                version = ReadInt32();
                if (version != 3)
                    throw new Exception(string.Format("Unknown string lookback version: {0}", version));
            }
        }
    }
}
