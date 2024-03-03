using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdWait : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01083000; }
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
                Condition.Block = value;
            }
        }

        public CMwCmdExpBool Condition
        {
            get { return ((Chunk000)GetChunk(0x01083000)).Condition; }
            set { ((Chunk000)GetChunk(0x01083000)).Condition = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpBool Condition;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Condition);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("{0}WaitIf({1});\r\n", GetIndent(indent), Condition);
        }
    }
}
