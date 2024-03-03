using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdArrayAdd : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x010A0000; }
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
                ParamInterface.Block = value;
                Value.Block = value;
            }
        }

        public CMwCmdParamInterface ParamInterface
        {
            get { return ((Chunk001)GetChunk(0x010A0001)).ParamInterface; }
            set { ((Chunk001)GetChunk(0x010A0001)).ParamInterface = value; }
        }

        public CMwCmdExp Value
        {
            get { return ((Chunk001)GetChunk(0x010A0001)).Value; }
            set { ((Chunk001)GetChunk(0x010A0001)).Value = value; }
        }

        public class Chunk001 : NodeChunk
        {
            public CMwCmdParamInterface ParamInterface;
            public CMwCmdExp Value;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref ParamInterface, 1);
                archive.ReadWriteNode(ref Value);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("{0}{1}.Add({2});\r\n", GetIndent(indent), ParamInterface, Value);
        }
    }
}
