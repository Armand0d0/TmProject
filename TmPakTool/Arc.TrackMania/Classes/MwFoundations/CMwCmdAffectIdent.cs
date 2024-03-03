using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdAffectIdent : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01032000; }
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
                Value.Block = value;
            }
        }

        public CMwCmdIdentInterface IdentInterface
        {
            get { return ((Chunk000)GetChunk(0x01032000)).IdentInterface; }
        }

        public CMwCmdExp Value
        {
            get { return ((Chunk000)GetChunk(0x01032000)).Value; }
            set { ((Chunk000)GetChunk(0x01032000)).Value = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdIdentInterface IdentInterface;
            public CMwCmdExp Value;

            public bool Const
            {
                get { return _const != 0; }
                set { _const = value ? 1u : 0u; }
            }

            private uint _const;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref IdentInterface);
                archive.ReadWriteNode(ref Value);
                archive.ReadWrite(ref _const);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("{0}{1} = {2};\r\n", GetIndent(indent), IdentInterface, Value);
        }
    }
}
