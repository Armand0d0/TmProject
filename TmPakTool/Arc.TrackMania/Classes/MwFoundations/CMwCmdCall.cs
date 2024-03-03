using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdCall : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01034000; }
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
            get { return ((Chunk001)GetChunk(0x01034001)).ParamInterface; }
            set { ((Chunk001)GetChunk(0x01034001)).ParamInterface = value; }
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
            return string.Format("{0}{1}();\r\n", GetIndent(indent), ParamInterface);
        }
    }
}
