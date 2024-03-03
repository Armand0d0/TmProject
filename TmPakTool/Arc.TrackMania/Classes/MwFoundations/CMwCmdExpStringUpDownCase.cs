using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpStringUpDownCase : CMwCmdExpString
    {
        public override uint ID
        {
            get { return 0x010A7000; }
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
                InputString.Block = value;
            }
        }

        public CMwCmdExpString InputString
        {
            get { return ((Chunk000)GetChunk(0x010A7000)).InputString; }
            set { ((Chunk000)GetChunk(0x010A7000)).InputString = value; }
        }

        public bool Upper
        {
            get { return ((Chunk000)GetChunk(0x010A7000)).Upper; }
            set { ((Chunk000)GetChunk(0x010A7000)).Upper = value; }
        }

        public class Chunk0104E000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpString InputString;
            public bool Upper
            {
                get { return _upper != 0; }
                set { _upper = value ? 1u : 0u; }
            }

            private uint _upper;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref InputString);
                archive.ReadWrite(ref _upper);
            }
        }

        public override string ToString(int indent)
        {
            if (Upper)
                return string.Format("{0}.Upper()", InputString);
            else
                return string.Format("{0}.Lower()", InputString);
        }
    }
}
