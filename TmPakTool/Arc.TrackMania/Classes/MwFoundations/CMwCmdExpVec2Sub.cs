using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpVec2Sub : CMwCmdExpVec2
    {
        public override uint ID
        {
            get { return 0x01086000; }
        }

        public override CMwCmdBlock Block
        {
            get
            {
                return base.Block;
            }
            set
            {
                base.Block = value;
                Vec1.Block = value;
                Vec2.Block = value;
            }
        }

        public CMwCmdExpVec2 Vec1
        {
            get { return ((Chunk000)GetChunk(0x01086000)).Vec1; }
            set { ((Chunk000)GetChunk(0x01086000)).Vec1 = value; }
        }

        public CMwCmdExpVec2 Vec2
        {
            get { return ((Chunk000)GetChunk(0x01086000)).Vec2; }
            set { ((Chunk000)GetChunk(0x01086000)).Vec2 = value; }
        }

        public class Chunk0105C000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpVec2 Vec1;
            public CMwCmdExpVec2 Vec2;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Vec1);
                archive.ReadWriteNode(ref Vec2);
            }
        }

        public override string ToString(int indent)
        {
            return BinOpToString("-", Vec1, Vec2);
        }
    }
}
