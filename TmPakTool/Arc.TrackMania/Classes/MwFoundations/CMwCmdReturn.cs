using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdReturn : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01094000; }
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
                if (ReturnValue != null)
                    ReturnValue.Block = value;
            }
        }

        public CMwCmd ReturnValue
        {
            get { return ((Chunk000)GetChunk(0x01094000)).ReturnValue; }
            set { ((Chunk000)GetChunk(0x01094000)).ReturnValue = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmd ReturnValue;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref ReturnValue);
            }
        }

        public override string ToString(int indent)
        {
            if (ReturnValue == null)
                return string.Format("{0}return;\r\n", GetIndent(indent));
            else
                return string.Format("{0}return {1};\r\n", GetIndent(indent), ReturnValue);
        }
    }
}
