using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpIso4Inverse : CMwCmdExpIso4
    {
        public override uint ID
        {
            get { return 0x01090000; }
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
                Matrix.Block = value;
            }
        }

        public CMwCmdExpIso4 Matrix
        {
            get { return ((Chunk01063000)GetChunk(0x01063000)).Matrix; }
            set { ((Chunk01063000)GetChunk(0x01063000)).Matrix = value; }
        }

        public class Chunk01062000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
        }

        public class Chunk01063000 : NodeChunk
        {
            public CMwCmdExpIso4 Matrix;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Matrix);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("{0}.Invert()", Matrix);
        }
    }
}
