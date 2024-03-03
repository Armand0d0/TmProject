using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpVec3Mult : CMwCmdExpVec3
    {
        public override uint ID
        {
            get { return 0x0108C000; }
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
                Factor.Block = value;
                Vec.Block = value;
            }
        }

        public CMwCmdExpNum Factor
        {
            get { return ((Chunk000)GetChunk(0x0108C000)).Factor; }
            set { ((Chunk000)GetChunk(0x0108C000)).Factor = value; }
        }

        public CMwCmdExpVec3 Vec
        {
            get { return ((Chunk000)GetChunk(0x0108C000)).Vec; }
            set { ((Chunk000)GetChunk(0x0108C000)).Vec = value; }
        }

        public class Chunk0105F000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpNum Factor;
            public CMwCmdExpVec3 Vec;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Factor);
                archive.ReadWriteNode(ref Vec);
            }
        }

        public override string ToString(int indent)
        {
            return BinOpToString("*", Factor, Vec);
        }
    }
}
