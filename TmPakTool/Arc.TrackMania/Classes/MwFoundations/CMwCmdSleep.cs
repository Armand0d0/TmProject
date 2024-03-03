using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdSleep : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01082000; }
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
                Time.Block = value;
            }
        }

        public CMwCmdExpNum Time
        {
            get { return ((Chunk000)GetChunk(0x01082000)).Time; }
            set { ((Chunk000)GetChunk(0x01082000)).Time = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpNum Time;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Time);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("{0}Sleep({1});\r\n", GetIndent(indent), Time);
        }
    }
}
