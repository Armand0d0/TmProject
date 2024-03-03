using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpEgal : CMwCmdExpBool
    {
        public override uint ID
        {
            get { return 0x01040000; }
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
                Value1.Block = value;
                Value2.Block = value;
            }
        }

        public CMwCmdExp Value1
        {
            get { return ((Chunk000)GetChunk(0x01040000)).Value1; }
            set { ((Chunk000)GetChunk(0x01040000)).Value1 = value; }
        }

        public CMwCmdExp Value2
        {
            get { return ((Chunk000)GetChunk(0x01040000)).Value2; }
            set { ((Chunk000)GetChunk(0x01040000)).Value2 = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExp Value1;
            public CMwCmdExp Value2;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Value1);
                archive.ReadWriteNode(ref Value2);
            }
        }

        public override string ToString(int indent)
        {
            return BinOpToString("==", Value1, Value2);
        }
    }
}
