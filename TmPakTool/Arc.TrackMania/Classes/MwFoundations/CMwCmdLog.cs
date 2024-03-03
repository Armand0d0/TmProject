using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdLog : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01084000; }
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
                Text.Block = value;
            }
        }

        public CMwCmdExpString Text
        {
            get { return ((Chunk000)GetChunk(0x01084000)).Text; }
            set { ((Chunk000)GetChunk(0x01084000)).Text = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpString Text;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Text);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("{0}Log({1});\r\n", GetIndent(indent), Text);
        }
    }
}
