using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpNumDotProduct3 : CMwCmdExpNum
    {
        public override uint ID
        {
            get { return 0x01092000; }
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
                Vec1.Block = value;
                Vec2.Block = value;
            }
        }

        public CMwCmdExpVec3 Vec1
        {
            get { return ((Chunk01055000)GetChunk(0x01055000)).Vec1; }
            set { ((Chunk01055000)GetChunk(0x01055000)).Vec1 = value; }
        }

        public CMwCmdExpVec3 Vec2
        {
            get { return ((Chunk01055000)GetChunk(0x01055000)).Vec2; }
            set { ((Chunk01055000)GetChunk(0x01055000)).Vec2 = value; }
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

        public class Chunk01055000 : NodeChunk
        {
            public CMwCmdExpVec3 Vec1;
            public CMwCmdExpVec3 Vec2;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Vec1);
                archive.ReadWriteNode(ref Vec2);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("Dot({0}, {1})", Vec1, Vec2);
        }
    }
}
