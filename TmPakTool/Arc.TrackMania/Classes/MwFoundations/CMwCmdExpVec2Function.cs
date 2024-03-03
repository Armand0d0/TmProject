using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpVec2Function : CMwCmdExpVec2
    {
        public override uint ID
        {
            get { return 0x0109E000; }
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
                FuncInterface.Block = value;
            }
        }

        public CMwCmdFunctionInterface FuncInterface
        {
            get { return ((Chunk000)GetChunk(0x0109E000)).FuncInterface; }
            set { ((Chunk000)GetChunk(0x0109E000)).FuncInterface = value; }
        }

        public class Chunk0105C000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdFunctionInterface FuncInterface;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref FuncInterface);
            }
        }

        public override string ToString(int indent)
        {
            return FuncInterface.ToString();
        }
    }
}
