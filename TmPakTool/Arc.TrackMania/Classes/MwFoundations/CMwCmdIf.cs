using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdIf : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01036000; }
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
                IfBlock.Block = value;
                if (ElseBlock != null)
                    ElseBlock.Block = value;
            }
        }

        public CMwCmdExpBool Condition
        {
            get { return ((Chunk000)GetChunk(0x01036000)).Condition; }
            set { ((Chunk000)GetChunk(0x01036000)).Condition = value; }
        }

        public CMwCmdBlock IfBlock
        {
            get { return ((Chunk000)GetChunk(0x01036000)).IfBlock; }
            set { ((Chunk000)GetChunk(0x01036000)).IfBlock = value; }
        }

        public CMwCmdBlock ElseBlock
        {
            get { return ((Chunk000)GetChunk(0x01036000)).ElseBlock; }
            set { ((Chunk000)GetChunk(0x01036000)).ElseBlock = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpBool Condition;
            public CMwCmdBlock IfBlock;
            public CMwCmdBlock ElseBlock;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Condition);
                archive.ReadWriteNode(ref IfBlock);
                archive.ReadWriteNode(ref ElseBlock);
            }
        }

        public override string ToString(int indent)
        {
            if (ElseBlock == null)
            {
                return string.Format("{0}if ({1})\r\n{2}", GetIndent(indent), Condition, IfBlock.ToString(indent));
            }
            else
            {
                string elseBlock;
                if (ElseBlock.Cmds.Count == 1 && ElseBlock.Cmds[0] is CMwCmdIf)
                {
                    elseBlock = " " + ElseBlock.Cmds[0].ToString(indent).TrimStart(' ');
                }
                else
                {
                    elseBlock = "\r\n" + ElseBlock.ToString(indent);
                }

                return string.Format("{0}if ({1})\r\n{2}{0}else{3}", GetIndent(indent), Condition,
                    IfBlock.ToString(indent), elseBlock);
            }
        }
    }
}
