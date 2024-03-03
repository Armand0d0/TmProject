using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes.MwFoundations;

namespace Arc.TrackMania.Classes.Game
{
    public class CGameCtnChallenge : Arc.TrackMania.Classes.MwFoundations.CMwNod
    {
        private static Dictionary<uint, uint> _chunkCompositions = new Dictionary<uint, uint>()
        {
            { 0x03043027, 0x03043028 }
        };

        public override uint ID
        {
            get { return 0x03043000; }
        }

        protected override CMwNod.ChunkFlags GetChunkFlags(uint chunkID)
        {
            switch (chunkID)
            {
                case 0x03043000:
                case 0x03043001:
                case 0x03043002:
                case 0x03043003:
                case 0x03043004:
                case 0x03043005:
                case 0x03043006:
                case 0x03043007:
                case 0x03043008:
                case 0x03043009:
                case 0x0304300B:
                case 0x0304300C:
                case 0x0304300E:
                case 0x0304300F:
                case 0x03043010:
                case 0x03043012:
                case 0x03043013:
                case 0x0304301A:
                case 0x0304301B:
                case 0x0304301D:
                case 0x0304301E:
                case 0x03043020:
                case 0x03043023:
                case 0x03043027:
                    return ChunkFlags.Known;

                case 0x0304300A:
                    return ChunkFlags.Known | ChunkFlags._Flag2;

                case 0x0304300D:
                case 0x03043011:
                case 0x0304301F:
                case 0x03043021:
                case 0x03043022:
                case 0x03043024:
                case 0x03043025:
                case 0x03043026:
                case 0x03043028:
                case 0x0304302A:
                    return ChunkFlags.Known | ChunkFlags._Flag1;

                case 0x03043014:
                case 0x03043015:
                case 0x03043016:
                    return ChunkFlags.Known | ChunkFlags.Skippable;

                case 0x03043017:
                case 0x03043018:
                case 0x03043019:
                case 0x0304301C:
                case 0x03043029:
                    return ChunkFlags.Known | ChunkFlags._Flag1 | ChunkFlags.Skippable;
            }
            return ChunkFlags.None;
        }

        protected override Dictionary<uint, uint> GetChunkCompositions()
        {
            return _chunkCompositions;
        }

        public class Chunk002 : NodeChunk
        {
            public byte Version;
            public uint Dword0;
            public uint BronzeTime;
            public uint SilverTime;
	        public uint GoldTime;
	        public uint AuthorTime;
	        public uint Price;
	        public uint Multilap;
	        public uint TrackType;
            public uint Dword8;
            public uint Dword9;
            public uint Dword10;

            public override bool IsHeaderChunk
            {
                get { return true; }
            }

            public override bool IsDetailHeaderChunk
            {
                get { return false; }
            }

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Version);
                archive.ReadWrite(ref Dword0);
                archive.ReadWrite(ref BronzeTime);
                archive.ReadWrite(ref SilverTime);
                archive.ReadWrite(ref GoldTime);
                archive.ReadWrite(ref AuthorTime);
                archive.ReadWrite(ref Price);
                archive.ReadWrite(ref Multilap);
                archive.ReadWrite(ref TrackType);
                archive.ReadWrite(ref Dword8);
                archive.ReadWrite(ref Dword9);
                if (Version >= 11)
                    archive.ReadWrite(ref Dword10);
            }
        }

        public class Chunk003 : NodeChunk
        {
            public byte Version;
            public Meta Meta0 = new Meta();
            public string TrackName;
            public byte Byte0;
            public uint Dword0;
            public string String0;
            public Meta Meta1 = new Meta();
            public float Float0;
            public float Float1;
            public float Float2;
            public float Float3;
            public byte[] DQWord0 = new byte[16];

            public override bool IsHeaderChunk
            {
                get { return true; }
            }

            public override bool IsDetailHeaderChunk
            {
                get { return false; }
            }

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Version);
                Meta0.ReadWrite(archive);
                archive.ReadWrite(ref TrackName);
                archive.ReadWrite(ref Byte0);
                archive.ReadWrite(ref Dword0);
                archive.ReadWrite(ref String0);
                Meta1.ReadWrite(archive);
                archive.ReadWrite(ref Float0);
                archive.ReadWrite(ref Float1);
                archive.ReadWrite(ref Float2);
                archive.ReadWrite(ref Float3);
                archive.ReadWrite(DQWord0);
            }
        }

        public class Chunk004 : NodeChunk
        {
            public uint Dword;

            public override bool IsHeaderChunk
            {
                get { return true; }
            }

            public override bool IsDetailHeaderChunk
            {
                get { return false; }
            }

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Dword);
            }
        }

        public class Chunk005 : NodeChunk
        {
            [FieldDisplay(Multiline = true)]
            public string XML;

            public override bool IsHeaderChunk
            {
                get { return true; }
            }

            public override bool IsDetailHeaderChunk
            {
                get { return true; }
            }

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref XML);
            }
        }

        public class Chunk007 : NodeChunk
        {
            private byte[] _thumbBytes;

            public Image Thumbnail
            {
                get
                {
                    if (_thumbBytes == null)
                        return null;

                    return Image.FromStream(new MemoryStream(_thumbBytes, false));
                }
                set
                {
                    MemoryStream stream = new MemoryStream();
                    value.Save(stream, ImageFormat.Jpeg);
                    
                    _thumbBytes = new byte[stream.Length];
                    stream.Position = 0;
                    stream.Read(_thumbBytes, 0, _thumbBytes.Length);
                }
            }
            public string Comment;

            public override bool IsHeaderChunk
            {
                get { return true; }
            }

            public override bool IsDetailHeaderChunk
            {
                get { return true; }
            }

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                int haveThumbnail = (_thumbBytes != null ? 1 : 0);
                archive.ReadWrite(ref haveThumbnail);
                if (haveThumbnail != 0)
                {
                    if (archive.Writing)
                        archive.Write(_thumbBytes.Length);
                    else
                        _thumbBytes = new byte[archive.ReadUInt32()];

                    archive.ReadWrite("<Thumbnail.jpg>");
                    archive.ReadWrite(_thumbBytes);
                    archive.ReadWrite("</Thumbnail.jpg>");
                    archive.ReadWrite("<Comments>");
                    archive.ReadWrite(ref Comment);
                    archive.ReadWrite("</Comments>");
                }
            }
        }

        public class Chunk00D : NodeChunk
        {
            public Meta Meta = new Meta();

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                Meta.ReadWrite(archive);
            }
        }

        public class Chunk011 : NodeChunk
        {
            public CGameCtnCollectorList CollectorList;
            public CGameCtnChallengeParameters Parameters;
            public uint Dword0;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref CollectorList);
                archive.ReadWriteNode(ref Parameters);
                archive.ReadWrite(ref Dword0);
            }
        }

        public class Chunk014 : NodeChunk
        {
            public uint Dword0;
            public string EncryptedPassword;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Dword0);
                archive.ReadWrite(ref EncryptedPassword);
            }
        }

        public class Chunk017 : NodeChunk
        {
            public class Item
            {
                public uint Dword0;
                public uint Dword1;
                public uint Dword2;
            }
            public BindingList<Item> Items = new BindingList<Item>();

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                int count = Items.Count;
                archive.ReadWrite(ref count);
                if (!archive.Writing)
                    Items = new BindingList<Item>();

                for (int i = 0; i < count; i++)
                {
                    if (!archive.Writing)
                        Items.Add(new Item());

                    archive.ReadWrite(ref Items[i].Dword0);
                    archive.ReadWrite(ref Items[i].Dword1);
                    archive.ReadWrite(ref Items[i].Dword2);
                }
            }
        }

        public class Chunk018 : NodeChunk
        {
            public uint Dword0;
            public uint Dword1;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Dword0);
                archive.ReadWrite(ref Dword1);
            }
        }

        public class Chunk019 : NodeChunk
        {
            public FileReference FileRef = new FileReference();

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                FileRef.ReadWrite(archive);
            }
        }

        public class Chunk01C : NodeChunk
        {
            public uint Dword0;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Dword0);
            }
        }

        public class Chunk01F : NodeChunk
        {
            public Meta TrackMeta = new Meta();
            public string TrackName;
            public Meta TimeMeta = new Meta();
            public uint Dword0;
            public uint Dword1;
            public uint Dword2;
            public uint Dword3;
            private uint FlagsAreDwords = 1;

            public BindingList<CGameCtnBlock> Blocks;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                TrackMeta.ReadWrite(archive);
                archive.ReadWrite(ref TrackName);
                TimeMeta.ReadWrite(archive);
                archive.ReadWrite(ref Dword0);
                archive.ReadWrite(ref Dword1);
                archive.ReadWrite(ref Dword2);
                archive.ReadWrite(ref Dword3);
                archive.ReadWrite(ref FlagsAreDwords);

                if (!archive.Writing)
                    Blocks = new BindingList<CGameCtnBlock>();

                int numBlocks = Blocks.Count;
                archive.ReadWrite(ref numBlocks);
                for (int i = 0; i < numBlocks; i++)
                {
                    if (!archive.Writing)
                        Blocks.Add(new CGameCtnBlock());

                    archive.ReadWriteLookbackString(ref Blocks[i].BlockName);
                    archive.ReadWrite(ref Blocks[i].Rotation);
                    archive.ReadWrite(ref Blocks[i].X);
                    archive.ReadWrite(ref Blocks[i].Y);
                    archive.ReadWrite(ref Blocks[i].Z);

                    if (FlagsAreDwords == 1)
                    {
                        archive.ReadWrite(ref Blocks[i].Flags);
                    }
                    else
                    {
                        if (archive.Writing)
                            archive.Write((ushort)Blocks[i].Flags);
                        else
                            Blocks[i].Flags = archive.ReadUInt16();
                    }

                    if (!archive.Writing && Blocks[i].CustomNode != null)
                        Blocks[i].Flags |= 0x8000;

                    if ((Blocks[i].Flags & 0x8000) != 0)
                    {
                        archive.ReadWriteLookbackString(ref Blocks[i].Author);
                        archive.ReadWriteNode(ref Blocks[i].CustomNode);
                    }
                }
            }
        }

        public class Chunk021 : NodeChunk
        {
            public CMwNod Node0;
            public CMwNod Node1;
            public CMwNod Node2;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Node0);
                archive.ReadWriteNode(ref Node1);
                archive.ReadWriteNode(ref Node2);
            }
        }

        public class Chunk022 : NodeChunk
        {
            public uint Dword0;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Dword0);
            }
        }

        public class Chunk024 : NodeChunk
        {
            public FileReference FileRef = new FileReference();

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                FileRef.ReadWrite(archive);
            }
        }

        public class Chunk025 : NodeChunk
        {
            public long Qword0;
            public long Qword1;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Qword0);
                archive.ReadWrite(ref Qword1);
            }
        }

        public class Chunk026 : NodeChunk
        {
            public CMwNod Node;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Node);
            }
        }

        public class Chunk027 : NodeChunk
        {
            public uint FilledIn;
            public byte Byte0;
            public Vec3D Vec0 = new Vec3D();
            public Vec3D Vec1 = new Vec3D();
            public Vec3D Vec2 = new Vec3D();
            public Vec3D Vec3 = new Vec3D();
            public float Float0;
            public float Float1;
            public float Float2;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref FilledIn);
                if (FilledIn != 0)
                {
                    archive.ReadWrite(ref Byte0);
                    Vec0.ReadWrite(archive);
                    Vec1.ReadWrite(archive);
                    Vec2.ReadWrite(archive);
                    Vec3.ReadWrite(archive);
                    archive.ReadWrite(ref Float0);
                    archive.ReadWrite(ref Float1);
                    archive.ReadWrite(ref Float2);
                }
            }
        }

        public class Chunk028 : NodeChunk
        {
            public string String0;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                node.ReadWriteChunk(archive, 0x03043027);
                archive.ReadWrite(ref String0);
            }
        }

        public class Chunk029 : NodeChunk
        {
            public byte[] PasswordHash = new byte[16];
            public uint PasswordHashCRC32;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(PasswordHash);
                archive.ReadWrite(ref PasswordHashCRC32);
            }
        }

        public class Chunk02A : NodeChunk
        {
            public uint Dword0;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Dword0);
            }
        }
    }
}
