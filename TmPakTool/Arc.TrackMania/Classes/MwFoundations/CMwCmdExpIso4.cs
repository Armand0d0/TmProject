using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpIso4 : CMwCmdExp
    {
        public override uint ID
        {
            get { return 0x01062000; }
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
                RotX.Block = value;
                RotY.Block = value;
                RotZ.Block = value;
                Pos.Block = value;
            }
        }

        public CMwCmdExpVec3 RotX
        {
            get { return ((Chunk000)GetChunk(0x01062000)).RotX; }
            set { ((Chunk000)GetChunk(0x01062000)).RotX = value; }
        }

        public CMwCmdExpVec3 RotY
        {
            get { return ((Chunk000)GetChunk(0x01062000)).RotY; }
            set { ((Chunk000)GetChunk(0x01062000)).RotY = value; }
        }

        public CMwCmdExpVec3 RotZ
        {
            get { return ((Chunk000)GetChunk(0x01062000)).RotZ; }
            set { ((Chunk000)GetChunk(0x01062000)).RotZ = value; }
        }

        public CMwCmdExpVec3 Pos
        {
            get { return ((Chunk000)GetChunk(0x01062000)).Pos; }
            set { ((Chunk000)GetChunk(0x01062000)).Pos = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpVec3 RotX;
            public CMwCmdExpVec3 RotY;
            public CMwCmdExpVec3 RotZ;
            public CMwCmdExpVec3 Pos;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref RotX);
                archive.ReadWriteNode(ref RotY);
                archive.ReadWriteNode(ref RotZ);
                archive.ReadWriteNode(ref Pos);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("Matrix43({0}, {1}, {2}, {3})", RotX, RotY, RotZ, Pos);
        }
    }
}
