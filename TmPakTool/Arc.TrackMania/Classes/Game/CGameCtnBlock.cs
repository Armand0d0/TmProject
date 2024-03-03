using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes.MwFoundations;

namespace Arc.TrackMania.Classes.Game
{
    public class CGameCtnBlock : Arc.TrackMania.Classes.MwFoundations.CMwNod
    {
        public string BlockName;
        public byte Rotation;
        public byte X;
        public byte Y;
        public byte Z;
        public uint Flags;
        public string Author;
        public CMwNod CustomNode = null;

        public override uint ID
        {
            get { return 0x03057000; }
        }

        protected override CMwNod.ChunkFlags GetChunkFlags(uint chunkID)
        {
            switch (chunkID)
            {
                case 0x3057000:
                case 0x3057001:
                case 0x3057002:
                    return ChunkFlags.Known;
            }
            return ChunkFlags.None;
        }

        public class Chunk000 : NodeChunk
        {
            public string BlockName;
            public int Rotation;
            public int X;
            public int Y;
            public int Z;
            public uint Flags0;
            public uint Flags1;
            public uint Flags2;
            public uint Flags3;
            public uint Flags4;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteLookbackString(ref BlockName);
                archive.ReadWrite(ref Rotation);
                archive.ReadWrite(ref X);
                archive.ReadWrite(ref Y);
                archive.ReadWrite(ref Z);
                archive.ReadWrite(ref Flags0);
                archive.ReadWrite(ref Flags1);
                archive.ReadWrite(ref Flags2);
                archive.ReadWrite(ref Flags3);
                archive.ReadWrite(ref Flags4);
            }
        }

        public class Chunk001 : NodeChunk
        {
            public Meta Meta = new Meta();
            public int Rotation;
            public int X;
            public int Y;
            public int Z;
            public uint Flags;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                Meta.ReadWrite(archive);
                archive.ReadWrite(ref Rotation);
                archive.ReadWrite(ref X);
                archive.ReadWrite(ref Y);
                archive.ReadWrite(ref Z);
                archive.ReadWrite(ref Flags);
            }
        }

        public class Chunk002 : NodeChunk
        {
            public Meta Meta = new Meta();
            public byte Rotation;
            public byte X;
            public byte Y;
            public byte Z;
            public uint Flags;
            public CMwNod CustomNode;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                Meta.ReadWrite(archive);
                archive.ReadWrite(ref Rotation);
                archive.ReadWrite(ref X);
                archive.ReadWrite(ref Y);
                archive.ReadWrite(ref Z);

                if(!archive.Writing)
                    Flags = CustomNode != null ? (uint)0x8000000 : 0;

                archive.ReadWrite(ref Flags);
                if ((Flags & 0x8000000) != 0)
                    archive.ReadWriteNode(ref CustomNode);
            }
        }
    }
}
