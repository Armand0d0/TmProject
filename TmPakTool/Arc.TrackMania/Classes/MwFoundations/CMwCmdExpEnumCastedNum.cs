using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpEnumCastedNum : CMwCmdExpEnum
    {
        public override uint ID
        {
            get { return 0x010A9000; }
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

        public new CMwCmdExpNum Value
        {
            get { return ((Chunk000)GetChunk(0x010A9000)).Value; }
            set { ((Chunk000)GetChunk(0x010A9000)).Value = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpNum Value;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Value);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("({0}){1}", CMwEngineManager.GetMemberInfo(EnumID).Name, Value);
        }
    }
}
