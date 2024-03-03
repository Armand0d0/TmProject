using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdSwitch : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x0105B000; }
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
                EnumParam.Block = value;
                foreach (CMwCmdBlock caseBlock in CaseArray)
                    caseBlock.Block = value;

                if (DefaultBlock != null)
                    DefaultBlock.Block = value;
            }
        }

        public CMwCmdExpEnum EnumParam
        {
            get { return ((Chunk000)GetChunk(0x0105B000)).EnumParam; }
            set { ((Chunk000)GetChunk(0x0105B000)).EnumParam = value; }
        }

        public List<CMwCmdBlock> CaseArray
        {
            get { return ((Chunk000)GetChunk(0x0105B000)).CaseArray; }
            set { ((Chunk000)GetChunk(0x0105B000)).CaseArray = value; }
        }

        public CMwCmdBlock DefaultBlock
        {
            get { return ((Chunk000)GetChunk(0x0105B000)).DefaultBlock; }
            set { ((Chunk000)GetChunk(0x0105B000)).DefaultBlock = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpEnum EnumParam;
            public List<CMwCmdBlock> CaseArray;
            public CMwCmdBlock DefaultBlock;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref EnumParam);
                archive.ReadWriteNodeList(ref CaseArray);
                archive.ReadWriteNode(ref DefaultBlock);
            }
        }

        public override string ToString(int indent)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}switch ({1})\r\n{0}{{\r\n", GetIndent(indent), EnumParam);
            for (int i = 0; i < CaseArray.Count; i++)
            {
                if (i > 0)
                    sb.Append("\r\n");

                sb.AppendFormat("{0}case {1}:\r\n{2}", GetIndent(indent + 1), i, CaseArray[i].ToString(indent + 1));
            }
            if (DefaultBlock != null)
            {
                if (CaseArray.Count > 0)
                    sb.Append("\r\n");

                sb.AppendFormat("{0}default:\r\n{1}", GetIndent(indent + 1), DefaultBlock.ToString(indent + 1));
            }
            sb.AppendFormat("{0}}}\r\n", GetIndent(indent));
            return sb.ToString();
        }
    }
}
