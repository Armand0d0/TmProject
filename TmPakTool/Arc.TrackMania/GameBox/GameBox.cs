using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes.MwFoundations;

namespace Arc.TrackMania.GameBox
{
    public class GameBox
    {
        private uint _classID;
        private byte[] _storageConfig;

        private CClassicArchive _archive = new CClassicArchive();

        public GameBox()
        {
            _storageConfig = new byte[] { (byte)'B', (byte)'U', (byte)'C', (byte)'R' };
        }

        public GameBox(Stream stream)
            : this()
        {
            Read(stream);
        }

        public GameBox(NadeoPak.NadeoPakFile nadeoPakFile)
            : this()
        {
            Read(nadeoPakFile);
        }

        public CMwNod MainNode
        {
            get { return _archive.MainNode; }
            set { _archive.MainNode = value; }
        }

        public List<CMwNod> Nodes
        {
            get { return _archive.Nodes; }
        }

        public void Read(Stream stream)
        {
            ReadWrite(new CClassicBuffer(stream, false), false);
        }

        public void Read(NadeoPak.NadeoPakFile nadeoPakFile)
        {
            ReadWrite(nadeoPakFile.GetBuffer(false), false);
        }

        public void Write(Stream stream)
        {
            ReadWrite(new CClassicBuffer(stream, true), false);
        }

        public void Write(NadeoPak.NadeoPakFile nadeoPakFile)
        {
            ReadWrite(nadeoPakFile.GetBuffer(true), false);
        }

        internal void ReadWrite(CClassicBuffer buffer, bool headerOnly)
        {
            _archive.ResetKnownStrings();
            _archive.PushBuffer(buffer);

            byte[] magic = new byte[] { (byte)'G', (byte)'B', (byte)'X' };
            _archive.ReadWrite(magic);
            if (magic[0] != 'G' || magic[1] != 'B' || magic[2] != 'X')
                throw new Exception("Bad GBX magic");

            short version = 6;
            _archive.ReadWrite(ref version);
            if (version != 6)
                throw new Exception(string.Format("Bad version (expecting 6, got {0})", version));

            _archive.ReadWrite(_storageConfig);
            if (_storageConfig[0] != 'B' && _storageConfig[0] != 'T')
                throw new Exception("Unknown storage mode '" + (char)_storageConfig[0] + "'");

            if (_storageConfig[1] != 'U' && _storageConfig[1] != 'C')
                throw new Exception("Unknown header compression setting '" + (char)_storageConfig[1] + "'");

            if (_storageConfig[2] != 'U' && _storageConfig[2] != 'C')
                throw new Exception("Unknown data compression setting '" + (char)_storageConfig[2] + "'");

            _archive.ReadWrite(ref _classID);
            _classID = CMwEngineManager.MapClassID(_classID);
            
            CMwNod mainNode;

            // Read/write header chunks
            if (_archive.Writing)
            {
                _archive.ResetNodesWritten();

                mainNode = _archive.MainNode;
                List<NodeChunk> headerChunks = new List<NodeChunk>();
                List<uint> headerChunkSizes = new List<uint>();

                _archive.PushBuffer(true);
                foreach (NodeChunk headerChunk in mainNode.HeaderChunks)
                {
                    uint offset = _archive.Position;
                    mainNode.ReadWriteChunk(_archive, headerChunk.ID);
                    uint length = _archive.Position - offset;

                    headerChunks.Add(headerChunk);
                    headerChunkSizes.Add(length);
                }
                byte[] headerChunksContent = _archive.PopBuffer();

                if (headerChunks.Count == 0)
                {
                    _archive.Write(0);
                }
                else
                {
                    uint headerSize = (uint)(4 + 8 * headerChunks.Count + headerChunksContent.Length);
                    _archive.Write(headerSize);
                    _archive.Write(headerChunks.Count);
                    for (int i = 0; i < headerChunks.Count; i++)
                    {
                        _archive.Write(headerChunks[i].ID);
                        if (headerChunks[i].IsDetailHeaderChunk)
                            _archive.Write(headerChunkSizes[i] | 0x80000000);
                        else
                            _archive.Write(headerChunkSizes[i]);
                    }
                    _archive.Write(headerChunksContent);
                }
            }
            else
            {
                uint headerSize = _archive.ReadUInt32();

                mainNode = CMwEngineManager.CreateClassInstance(_classID);
                if (mainNode == null)
                {
                    throw new Exception(string.Format("Class ID {0} ({1}) is not supported",
                        _classID, CMwEngineManager.GetClassName(_classID)));
                }

                if (headerSize > 0)
                {
                    uint numHeaderChunks = _archive.ReadUInt32();
                    List<uint> headerChunkIDs = new List<uint>();
                    for (uint i = 0; i < numHeaderChunks; i++)
                    {
                        uint chunkID = _archive.ReadUInt32();
                        uint chunkSize = _archive.ReadUInt32() & 0x7FFFFFFF;
                        headerChunkIDs.Add(chunkID);
                    }

                    foreach (uint chunkID in headerChunkIDs)
                    {
                        mainNode.ReadWriteChunk(_archive, chunkID);
                    }
                }
            }

            _archive.ResetKnownStrings();

            // Read/write data
            if (_archive.Writing)
            {
                _archive.PushBuffer(true);
                mainNode.ReadWrite(_archive);
                byte[] uncompressed = _archive.PopBuffer();

                _archive.Write(_archive.Nodes.Count);
                _archive.Write(0);

                if (!headerOnly)
                {
                    if (DataCompressed)
                    {
                        byte[] compressed = Compression.LZO.Compress(uncompressed);
                        _archive.Write(uncompressed.Length);
                        _archive.Write(compressed.Length);
                        _archive.Write(compressed);
                    }
                    else
                    {
                        _archive.Write(uncompressed);
                    }
                }
            }
            else
            {
                int numNodes = _archive.ReadInt32();
                _archive.Init(mainNode, numNodes);

                if (!headerOnly)
                {
                    int specialSize = _archive.ReadInt32();
                    if (specialSize != 0)
                        throw new Exception("Special data size != 0 not supported");

                    if (DataCompressed)
                    {
                        uint uncompressedSize = _archive.ReadUInt32();
                        uint compressedSize = _archive.ReadUInt32();
                        byte[] compressed = _archive.ReadBytes(compressedSize);
                        byte[] uncompressed = Compression.LZO.Decompress(compressed, uncompressedSize);

                        _archive.PushBuffer(uncompressed);
                    }

                    mainNode.ReadWrite(_archive);

                    if (DataCompressed)
                        _archive.PopBuffer();
                }
            }

            _archive.PopBuffer();
        }

        public uint ClassID
        {
            get { return _classID; }
        }

        public string ClassName
        {
            get
            {
                return CMwEngineManager.GetClassName(_classID);
            }
        }

        public bool HeaderCompressed
        {
            get { return _storageConfig[1] == 'C'; }
        }

        public bool DataCompressed
        {
            get { return _storageConfig[2] == 'C'; }
        }
    }
}
