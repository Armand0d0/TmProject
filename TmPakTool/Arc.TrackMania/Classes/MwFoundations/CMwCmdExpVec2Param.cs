using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpVec2Param : CMwCmdExpVec2
    {
        public override uint ID
        {
            get { return 0x0105E000; }
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
            get { return ((Chunk001)GetChunk(0x0105E001)).ParamInterface; }
            set { ((Chunk001)GetChunk(0x0105E001)).ParamInterface = value; }
        }

        public class Chunk0105C000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
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