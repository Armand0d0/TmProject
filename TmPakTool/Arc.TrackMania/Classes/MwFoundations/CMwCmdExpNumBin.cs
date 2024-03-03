using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpNumBin : CMwCmdExpNum
    {
        public override uint ID
        {
            get { return 0x01053000; }
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

        public override bool IsFloat
        {
            get { return ((Chunk01046000)GetChunk(0x01046000)).IsFloat; }
            set { ((Chunk01046000)GetChunk(0x01046000)).IsFloat = value; }
        }

        public override int IntValue
        {
            get { return 0; }
            set { }
        }

        public override float FloatValue
        {
            get { return 0.0f; }
            set { }
        }

        public CMwCmdExp Value1
        {
            get { return ((Chunk000)GetChunk(0x01053000)).Value1; }
            set { ((Chunk000)GetChunk(0x01053000)).Value1 = value; }
        }

        public CMwCmdExp Value2
        {
            get { return ((Chunk000)GetChunk(0x01053000)).Value2; }
            set { ((Chunk000)GetChunk(0x01053000)).Value2 = value; }
        }

        public class Chunk01046000 : NodeChunk
        {
            private uint _isFloat;

            public bool IsFloat
            {
                get { return _isFloat != 0; }
                set { _isFloat = value ? 1u : 0u; }
            }

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref _isFloat);
            }
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
    }
}
