using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpNum : CMwCmdExp
    {
        public override uint ID
        {
            get { return 0x01046000; }
        }

        public virtual bool IsFloat
        {
            get { return ((Chunk000)GetChunk(0x01046000)).IsFloat; }
            set { ((Chunk000)GetChunk(0x01046000)).IsFloat = value; }
        }

        public virtual int IntValue
        {
            get { return ((Chunk000)GetChunk(0x01046000)).IntValue; }
            set { ((Chunk000)GetChunk(0x01046000)).IntValue = value; }
        }

        public virtual float FloatValue
        {
            get { return ((Chunk000)GetChunk(0x01046000)).FloatValue; }
            set { ((Chunk000)GetChunk(0x01046000)).FloatValue = value; }
        }

        public class Chunk000 : NodeChunk
        {
            private uint _isFloat;

            public bool IsFloat
            {
                get { return _isFloat != 0; }
                set { _isFloat = value ? 1u : 0u; }
            }

            public int IntValue;
            public float FloatValue;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref _isFloat);
                if (IsFloat)
                    archive.ReadWrite(ref FloatValue);
                else
                    archive.ReadWrite(ref IntValue);
            }
        }

        public override string ToString(int indent)
        {
            if (IsFloat)
                return string.Format("{0:0.####}", FloatValue);
            else
                return string.Format("{0}", IntValue);
        }
    }
}
