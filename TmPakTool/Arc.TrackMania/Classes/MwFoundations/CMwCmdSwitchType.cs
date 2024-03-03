using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdSwitchType : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01066000; }
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
                foreach (CMwCmdBlock caseBlock in CaseArray)
                    caseBlock.Block = value;

                if (DefaultBlock != null)
                    DefaultBlock.Block = value;
            }
        }

        public string VarName
        {
            get { return ((Chunk000)GetChunk(0x01066000)).VarName; }
            set { ((Chunk000)GetChunk(0x01066000)).VarName = value; }
        }

        public List<uint> ClassIDs
        {
            get { return ((Chunk000)GetChunk(0x01066000)).ClassIDs; }
            set { ((Chunk000)GetChunk(0x01066000)).ClassIDs = value; }
        }

        public List<CMwCmdBlock> CaseArray
        {
            get { return ((Chunk000)GetChunk(0x01066000)).CaseArray; }
            set { ((Chunk000)GetChunk(0x01066000)).CaseArray = value; }
        }

        public CMwCmdBlock DefaultBlock
        {
            get { return ((Chunk000)GetChunk(0x01066000)).DefaultBlock; }
            set { ((Chunk000)GetChunk(0x01066000)).DefaultBlock = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public string VarName;
            public List<uint> ClassIDs = new List<uint>();
            public List<CMwCmdBlock> CaseArray;
            public CMwCmdBlock DefaultBlock;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref VarName);
                archive.ReadWrite(ref ClassIDs);
                archive.ReadWriteNodeList(ref CaseArray);
                archive.ReadWriteNode(ref DefaultBlock);
            }
        }

        public override string ToString(int indent)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < CaseArray.Count; i++)
            {
                sb.AppendFormat(GetIndent(indent));
                if (i == 0)
                    sb.Append("if ");
                else
                    sb.Append("else if ");

                sb.AppendFormat("({0} is {1})\r\n{2}", VarName, CMwEngineManager.GetClassInfo(ClassIDs[i]).Name,
                    CaseArray[i].ToString(indent));
            }
            if (DefaultBlock != null)
            {
                if (CaseArray.Count > 0)
                    sb.AppendFormat("{0}else\r\n", GetIndent(indent));

                sb.Append(DefaultBlock.ToString(indent));
            }
            return sb.ToString();
        }
    }
}
