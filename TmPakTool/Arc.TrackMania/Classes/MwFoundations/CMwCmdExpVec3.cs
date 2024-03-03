using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpVec3 : CMwCmdExp
    {
        public override uint ID
        {
            get { return 0x0105F000; }
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
                Z.Block = value;
            }
        }

        public CMwCmdExpNum X
        {
            get { return ((Chunk000)GetChunk(0x0105F000)).X; }
            set { ((Chunk000)GetChunk(0x0105F000)).X = value; }
        }

        public CMwCmdExpNum Y
        {
            get { return ((Chunk000)GetChunk(0x0105F000)).Y; }
            set { ((Chunk000)GetChunk(0x0105F000)).Y = value; }
        }

        public CMwCmdExpNum Z
        {
            get { return ((Chunk000)GetChunk(0x0105F000)).Z; }
            set { ((Chunk000)GetChunk(0x0105F000)).Z = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpNum X;
            public CMwCmdExpNum Y;
            public CMwCmdExpNum Z;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref X);
                archive.ReadWriteNode(ref Y);
                archive.ReadWriteNode(ref Z);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("Vec3({0}, {1}, {2})", X, Y, Z);
        }
    }
}
