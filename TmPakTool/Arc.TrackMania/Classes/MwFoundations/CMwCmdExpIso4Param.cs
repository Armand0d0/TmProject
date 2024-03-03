using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpIso4Param : CMwCmdExpIso4
    {
        public override uint ID
        {
            get { return 0x01064000; }
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
            }
        }

        public CMwCmdParamInterface ParamInterface
        {
            get { return ((Chunk001)GetChunk(0x01064001)).ParamInterface; }
            set { ((Chunk001)GetChunk(0x01064001)).ParamInterface = value; }
        }

        public class Chunk001 : NodeChunk
        {
            public CMwCmdParamInterface ParamInterface;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref ParamInterface, 1);
            }
        }

        public override string ToString(int indent)
        {
            return ParamInterface.ToString();
        }
    }
}