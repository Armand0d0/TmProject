using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdWhile : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01037000; }
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
                Condition.Block = value;
                Body.Block = value;
            }
        }

        public CMwCmdExpBool Condition
        {
            get { return ((Chunk000)GetChunk(0x01037000)).Condition; }
            set { ((Chunk000)GetChunk(0x01037000)).Condition = value; }
        }

        public CMwCmdBlock Body
        {
            get { return ((Chunk000)GetChunk(0x01037000)).Body; }
            set { ((Chunk000)GetChunk(0x01037000)).Body = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpBool Condition;
            public CMwCmdBlock Body;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Condition);
                archive.ReadWriteNode(ref Body);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("{0}while ({1})\r\n{2}", GetIndent(indent), Condition, Body.ToString(indent));
        }
    }
}
