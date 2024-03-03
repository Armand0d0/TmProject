using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpNumIdent : CMwCmdExpNum
    {
        public override uint ID
        {
            get { return 0x01047000; }
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
                IdentInterface.Block = value;
            }
        }

        public CMwCmdIdentInterface IdentInterface
        {
            get { return ((Chunk001)GetChunk(0x01047001)).IdentInterface; }
            set { ((Chunk001)GetChunk(0x01047001)).IdentInterface = value; }
        }

        public int Index
        {
            get { return ((Chunk001)GetChunk(0x01047001)).Index; }
            set { ((Chunk001)GetChunk(0x01047001)).Index = value; }
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

        public class Chunk001 : NodeChunk
        {
            public CMwCmdIdentInterface IdentInterface;
            public int Index;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref IdentInterface);
                archive.ReadWrite(ref Index);
            }
        }

        public override string ToString(int indent)
        {
            return IdentInterface.ToString() + GetNumField(IdentInterface.Variable.Type, Index);
        }
    }
}
