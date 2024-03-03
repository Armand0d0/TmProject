using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpIso4Mult : CMwCmdExpIso4
    {
        public override uint ID
        {
            get { return 0x0108F000; }
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
                Matrix1.Block = value;
                Matrix2.Block = value;
            }
        }

        public CMwCmdExpIso4 Matrix1
        {
            get { return ((Chunk01063000)GetChunk(0x01063000)).Matrix1; }
            set { ((Chunk01063000)GetChunk(0x01063000)).Matrix1 = value; }
        }

        public CMwCmdExpIso4 Matrix2
        {
            get { return ((Chunk01063000)GetChunk(0x01063000)).Matrix2; }
            set { ((Chunk01063000)GetChunk(0x01063000)).Matrix2 = value; }
        }

        public class Chunk01062000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
        }

        public class Chunk01063000 : NodeChunk
        {
            public CMwCmdExpIso4 Matrix1;
            public CMwCmdExpIso4 Matrix2;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Matrix1);
                archive.ReadWriteNode(ref Matrix2);
            }
        }

        public override string ToString(int indent)
        {
            return BinOpToString("*", Matrix1, Matrix2);
        }
    }
}
