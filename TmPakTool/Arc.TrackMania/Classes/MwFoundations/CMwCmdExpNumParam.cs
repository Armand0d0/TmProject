using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpNumParam : CMwCmdExpNum
    {
        public override uint ID
        {
            get { return 0x01048000; }
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
            get { return ((Chunk001)GetChunk(0x01048001)).ParamInterface; }
            set { ((Chunk001)GetChunk(0x01048001)).ParamInterface = value; }
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
            public CMwCmdParamInterface ParamInterface;
            public uint Dword0;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref ParamInterface, 1);
                archive.ReadWrite(ref Dword0);
            }
        }

        public override string ToString(int indent)
        {
            return ParamInterface.ToString();
        }
    }
}
