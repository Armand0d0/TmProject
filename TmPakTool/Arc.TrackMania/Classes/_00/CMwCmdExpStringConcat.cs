using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes.MwFoundations;

namespace Arc.TrackMania.Classes._00
{
    public class CMwCmdExpStringConcat : CMwCmdExpString
    {
        public override uint ID
        {
            get { return 0x00105100; }
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
            get { return ((Chunk100)GetChunk(0x00105100)).Value1; }
            set { ((Chunk100)GetChunk(0x00105100)).Value1 = value; }
        }

        public CMwCmdExp Value2
        {
            get { return ((Chunk100)GetChunk(0x00105100)).Value2; }
            set { ((Chunk100)GetChunk(0x00105100)).Value2 = value; }
        }

        public class Chunk0104E000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
        }

        public class Chunk100 : NodeChunk
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
            return string.Format("{0} .. {1}", Value1, Value2);
        }
    }
}
