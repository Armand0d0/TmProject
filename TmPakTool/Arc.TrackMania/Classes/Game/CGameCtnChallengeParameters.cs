using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes.MwFoundations;

namespace Arc.TrackMania.Classes.Game
{
    public class CGameCtnChallengeParameters : Arc.TrackMania.Classes.MwFoundations.CMwNod
    {
        public override uint ID
        {
            get { return 0x305B000; }
        }

        protected override CMwNod.ChunkFlags GetChunkFlags(uint chunkID)
        {
            switch (chunkID)
            {
                case 0x305B000:
                case 0x305B002:
                case 0x305B003:
                case 0x305B005:
                case 0x305B006:
                case 0x305B007:
                    return ChunkFlags.Known;

                case 0x305B001:
                case 0x305B004:
                case 0x305B008:
                    return ChunkFlags.Known | ChunkFlags._Flag1;
            }
            return ChunkFlags.None;
        }

        public class Chunk000 : NodeChunk
        {
            public uint Dword0;
            public uint Dword1;
            public uint Dword2;
            public uint Dword3;

            public uint Dword4;
            public uint Dword5;
            public uint Dword6;

            public uint Dword7;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Dword0);
                archive.ReadWrite(ref Dword1);
                archive.ReadWrite(ref Dword2);
                archive.ReadWrite(ref Dword3);
                archive.ReadWrite(ref Dword4);
                archive.ReadWrite(ref Dword5);
                archive.ReadWrite(ref Dword6);
                archive.ReadWrite(ref Dword7);
            }
        }

        public class Chunk001 : NodeChunk
        {
            public string String0;
            public string String1;
            public string String2;
            public string String3;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref String0);
                archive.ReadWrite(ref String1);
                archive.ReadWrite(ref String2);
                archive.ReadWrite(ref String3);
            }
        }

        public class Chunk002 : NodeChunk
        {
            public uint Dword0;
            public uint Dword1;
            public uint Dword2;
            public float Float0;
            public float Float1;
            public float Float2;
            public uint Dword3;
            public uint Dword4;
            public uint Dword5;
            public uint Dword6;
            public uint Dword7;
            public uint Dword8;
            public uint Dword9;
            public uint DwordA;
            public uint DwordB;
            public uint DwordC;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Dword0);
                archive.ReadWrite(ref Dword1);
                archive.ReadWrite(ref Dword2);
                archive.ReadWrite(ref Float0);
                archive.ReadWrite(ref Float1);
                archive.ReadWrite(ref Float2);
                archive.ReadWrite(ref Dword3);
                archive.ReadWrite(ref Dword4);
                archive.ReadWrite(ref Dword5);
                archive.ReadWrite(ref Dword6);
                archive.ReadWrite(ref Dword7);
                archive.ReadWrite(ref Dword8);
                archive.ReadWrite(ref Dword9);
                archive.ReadWrite(ref DwordA);
                archive.ReadWrite(ref DwordB);
                archive.ReadWrite(ref DwordC);
            }
        }

        public class Chunk003 : NodeChunk
        {
            public uint Dword0;
            public float Float0;

            public uint Dword1;
            public uint Dword2;
            public uint Dword3;

            public uint Dword4;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Dword0);
                archive.ReadWrite(ref Float0);
                archive.ReadWrite(ref Dword1);
                archive.ReadWrite(ref Dword2);
                archive.ReadWrite(ref Dword3);
                archive.ReadWrite(ref Dword4);
            }
        }

        public class Chunk004 : NodeChunk
        {
            public uint BronzeTime;
            public uint SilverTime;
            public uint GoldTime;
            public uint AuthorTime;
            public uint Dword0;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref BronzeTime);
                archive.ReadWrite(ref SilverTime);
                archive.ReadWrite(ref GoldTime);
                archive.ReadWrite(ref AuthorTime);
                archive.ReadWrite(ref Dword0);
            }
        }

        public class Chunk005 : NodeChunk
        {
            public uint Dword0;
            public uint Dword1;
            public uint Dword2;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Dword0);
                archive.ReadWrite(ref Dword1);
                archive.ReadWrite(ref Dword2);
            }
        }

        public class Chunk006 : NodeChunk
        {
            public BindingList<uint> List;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                if (archive.Writing)
                {
                    archive.Write(List.Count);
                    foreach (uint item in List)
                    {
                        archive.Write(item);
                    }
                }
                else
                {
                    int count = archive.ReadInt32();
                    List = new BindingList<uint>();
                    for (int i = 0; i < count; i++)
                    {
                        List.Add(archive.ReadUInt32());
                    }
                }
            }
        }

        public class Chunk008 : NodeChunk
        {
            public uint Dword0;
            public uint Dword1;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Dword0);
                archive.ReadWrite(ref Dword1);
            }
        }
    }
}
