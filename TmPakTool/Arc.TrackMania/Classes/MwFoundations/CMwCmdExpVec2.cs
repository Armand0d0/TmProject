using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpVec2 : CMwCmdExp
    {
        public override uint ID
        {
            get { return 0x0105C000; }
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
                X.Block = value;
                Y.Block = value;
            }
        }

        public CMwCmdExpNum X
        {
            get { return ((Chunk000)GetChunk(0x0105C000)).X; }
            set { ((Chunk000)GetChunk(0x0105C000)).X = value; }
        }

        public CMwCmdExpNum Y
        {
            get { return ((Chunk000)GetChunk(0x0105C000)).Y; }
            set { ((Chunk000)GetChunk(0x0105C000)).Y = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpNum X;
            public CMwCmdExpNum Y;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref X);
                archive.ReadWriteNode(ref Y);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("Vec2({0}, {1})", X, Y);
        }
    }
}
