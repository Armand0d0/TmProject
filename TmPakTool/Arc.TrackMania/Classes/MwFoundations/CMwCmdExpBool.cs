using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpBool : CMwCmdExp
    {
        public override uint ID
        {
            get { return 0x0103B000; }
        }

        public bool Value
        {
            get { return ((Chunk000)GetChunk(0x0103B000)).Value; }
            set { ((Chunk000)GetChunk(0x0103B000)).Value = value; }
        }

        public class Chunk000 : NodeChunk
        {
            private uint _value;

            public bool Value
            {
                get { return _value != 0; }
                set { _value = value ? 1u : 0u; }
            }

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref _value);
            }
        }

        public override string ToString(int indent)
        {
            return Value ? "true" : "false";
        }
    }
}
