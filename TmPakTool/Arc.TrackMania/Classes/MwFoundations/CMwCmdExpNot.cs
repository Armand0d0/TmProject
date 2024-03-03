using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpNot : CMwCmdExpBool
    {
        public override uint ID
        {
            get { return 0x01045000; }
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

        public CMwCmd Value
        {
            get { return ((Chunk000)GetChunk(0x01045000)).Value; }
            set { ((Chunk000)GetChunk(0x01045000)).Value = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmd Value;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Value);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("!{0}", Value);
        }
    }
}
