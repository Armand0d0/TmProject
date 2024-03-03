using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpStringParam : CMwCmdExpString
    {
        public override uint ID
        {
            get { return 0x01050000; }
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
            get { return ((Chunk002)GetChunk(0x01050002)).ParamInterface; }
            set { ((Chunk002)GetChunk(0x01050002)).ParamInterface = value; }
        }

        public class Chunk0104E000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
        }

        public class Chunk002 : NodeChunk
        {
            public CMwCmdParamInterface ParamInterface;
            public uint Num;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref ParamInterface, 1);
                archive.ReadWrite(ref Num);
            }
        }

        public override string ToString(int indent)
        {
            return ParamInterface.ToString();
        }
    }
}
