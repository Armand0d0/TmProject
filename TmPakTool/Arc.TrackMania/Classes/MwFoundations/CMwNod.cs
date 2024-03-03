using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    [TypeDescriptionProvider(typeof(FieldTypeDescriptionProvider<CMwNod>))]
    public abstract class CMwNod : IReadWrite
    {
        private List<NodeChunk> _chunks = new List<NodeChunk>();

        public virtual uint ID
        {
            get { return 0x01001000; }
        }

        public string Name
        {
            get { return CMwEngineManager.GetClassName(ID); }
        }

        public List<NodeChunk> Chunks { get { return _chunks; } }

        public IEnumerable<NodeChunk> HeaderChunks
        {
            get { return (from NodeChunk chunk in Chunks where chunk.IsHeaderChunk select chunk); }
        }

        public IEnumerable<NodeChunk> DataChunks
        {
            get { return (from NodeChunk chunk in Chunks where !chunk.IsHeaderChunk select chunk); }
        }

        protected enum ChunkFlags
        {
            None = 0,
            Known = 1,
            _Flag1 = 2,
            _Flag2 = 4,
            Skippable = 16
        }

        protected virtual ChunkFlags GetChunkFlags(uint chunkID)
        {
            return ChunkFlags.Known;
        }

        protected virtual Dictionary<uint, uint> GetChunkCompositions()
        {
            return null;
        }

        public NodeChunk GetChunk(uint chunkID)
        {
            return (from NodeChunk chunk in Chunks where chunk.ID == chunkID select chunk).FirstOrDefault();
        }

        public NodeChunk CreateChunk(uint chunkID)
        {
            Type classType = null;
            Type chunkType = null;
            chunkID = CMwEngineManager.MapClassID(chunkID);
            if ((chunkID & 0xFFFFF000) != ID)
            {
                classType = GetType();
                while (chunkType == null && classType != null)
                {
                    chunkType = Type.GetType(string.Format("{0}.{1}+Chunk{2:X08}", classType.Namespace,
                        classType.Name, chunkID));
                    if (chunkType == null)
                        classType = classType.BaseType;
                }
            }

            if (chunkType == null)
            {
                CMwClassInfo classInfo = CMwEngineManager.GetClassInfo(chunkID & 0xFFFFF000);
                if (classInfo == null)
                    throw new Exception(string.Format("CreateChunk: invalid chunkID {0:X08}", chunkID));

                classType = classInfo.ClassType;
                chunkType = Type.GetType(string.Format("{0}.{1}+Chunk{2:X03}", classType.Namespace,
                    classType.Name, chunkID & 0xFFF));
            }

            if (chunkType == null)
                throw new Exception(string.Format("CreateChunk: invalid chunkID {0:X08}", chunkID));

            NodeChunk chunk = Activator.CreateInstance(chunkType) as NodeChunk;
            chunk.Node = this;
            return chunk;
        }

        public void Read(Stream stream)
        {
            ReadWrite(new CClassicArchive(new CClassicBuffer(stream, false)));
        }

        public void Read(NadeoPak.NadeoPakFile nadeoPakFile)
        {
            ReadWrite(new CClassicArchive(nadeoPakFile.GetBuffer(false)));
        }

        public void Write(Stream stream)
        {
            ReadWrite(new CClassicArchive(new CClassicBuffer(stream, true)));
        }

        public void Write(NadeoPak.NadeoPakFile nadeoPakFile)
        {
            ReadWrite(new CClassicArchive(nadeoPakFile.GetBuffer(true)));
        }

        internal void ReadWriteChunk(CClassicArchive archive, uint chunkID)
        {
            NodeChunk chunk = GetChunk(chunkID);
            if (!archive.Writing && chunk == null)
                chunk = CreateChunk(chunkID);

            chunk.ReadWrite(archive, this);
            if (!archive.Writing)
                _chunks.Add(chunk);
        }

        internal override void ReadWrite(CClassicArchive archive)
        {
            Type baseType = GetType().BaseType;
            if (baseType != null)
            {
                uint parentClassID = CMwEngineManager.ReverseMapClassID(CMwEngineManager.GetClassInfo(baseType).ID);
                if (parentClassID == 0x07031000)
                    parentClassID = 0x07001000;

                byte[] parentClassIDBytes = BitConverter.GetBytes(parentClassID);
                archive.Buffer.Initialize(parentClassIDBytes, 0, 4);
            }

            CClassicBuffer buffer = archive.Buffer;
            if (buffer.Writing)
            {
                Dictionary<uint, uint> chunkCompositions = GetChunkCompositions();
                foreach (NodeChunk chunk in DataChunks)
                {
                    uint chunkID = CMwEngineManager.MapClassID(chunk.ID);
                    if (chunkCompositions != null && chunkCompositions.ContainsKey(chunkID) &&
                        GetChunk(chunkCompositions[chunkID]) != null)
                    {
                        continue;
                    }

                    archive.Write(chunkID);
                    ChunkFlags chunkFlags = GetChunkFlags(chunkID);
                    if ((chunkFlags & ChunkFlags.Known) == ChunkFlags.None)
                        throw new Exception(string.Format("Unknown chunk ID: {0}", chunkID));

                    if ((chunkFlags & ChunkFlags.Skippable) != ChunkFlags.None)
                    {
                        archive.PushBuffer(true);
                        ReadWriteChunk(archive, chunkID);
                        byte[] chunkContent = archive.PopBuffer();

                        archive.Write(0x534B4950);    // 'SKIP'
                        archive.Write(chunkContent.Length);
                        archive.Write(chunkContent);
                    }
                    else
                    {
                        ReadWriteChunk(archive, chunkID);
                    }
                }
                archive.Write(0xFACADE01);
            }
            else
            {
                uint chunkID;
                while (true)
                {
                    chunkID = archive.ReadUInt32();
                    if (chunkID == 0xFACADE01)
                        break;

                    chunkID = CMwEngineManager.MapClassID(chunkID);
                    ChunkFlags chunkFlags = GetChunkFlags(chunkID);
                    if ((chunkFlags & ChunkFlags.Known) == ChunkFlags.None)
                        throw new Exception(string.Format("Unknown chunk ID: {0}", chunkID));

                    if ((chunkFlags & ChunkFlags.Skippable) != ChunkFlags.None)
                    {
                        archive.ReadUInt32();
                        archive.ReadUInt32();
                    }
                    ReadWriteChunk(archive, chunkID);
                }
            }
        }
    }
}
