using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpVec3Neg : CMwCmdExpVec3
    {
        public override uint ID
        {
            get { return 0x0108B000; }
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
            }
        }

        public CMwCmdExpVec3 Vec
        {
            get { return ((Chunk000)GetChunk(0x0108B000)).Vec; }
            set { ((Chunk000)GetChunk(0x0108B000)).Vec = value; }
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

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Vec);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("-{0}", Vec);
        }
    }
}
