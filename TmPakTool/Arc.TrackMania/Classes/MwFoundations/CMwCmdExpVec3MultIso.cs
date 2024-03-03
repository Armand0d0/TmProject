using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpVec3MultIso : CMwCmdExpVec3
    {
        public override uint ID
        {
            get { return 0x0108D000; }
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
                Vec.Block = value;
                Iso4.Block = value;
            }
        }

        public CMwCmdExpVec3 Vec
        {
            get { return ((Chunk000)GetChunk(0x0108D000)).Vec; }
            set { ((Chunk000)GetChunk(0x0108D000)).Vec = value; }
        }

        public CMwCmdExpIso4 Iso4
        {
            get { return ((Chunk000)GetChunk(0x0108D000)).Iso4; }
            set { ((Chunk000)GetChunk(0x0108D000)).Iso4 = value; }
        }

        public class Chunk0105F000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpVec3 Vec;
            public CMwCmdExpIso4 Iso4;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Vec);
                archive.ReadWriteNode(ref Iso4);
            }
        }

        public override string ToString(int indent)
        {
            return BinOpToString("*", Vec, Iso4);
        }
    }
}
