using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdBreak : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01095000; }
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
                Target.Block = value;
            }
        }

        public CMwCmd Target
        {
            get { return ((Chunk000)GetChunk(0x01095000)).Target; }
            set { ((Chunk000)GetChunk(0x01095000)).Target = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmd Target;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Target);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("{0}break;\r\n", GetIndent(indent));
        }
    }
}
