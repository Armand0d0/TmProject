using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpNumCastedEnum : CMwCmdExpNum
    {
        public override uint ID
        {
            get { return 0x010A8000; }
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
                Value.Block = value;
            }
        }

        public CMwCmdExpEnum Value
        {
            get { return ((Chunk000)GetChunk(0x010A8000)).Value; }
            set { ((Chunk000)GetChunk(0x010A8000)).Value = value; }
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
            public CMwCmdExpEnum Value;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Value);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("(int){0}", Value);
        }
    }
}
