using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdBlock : CMwCmdScript
    {
        public override uint ID
        {
            get { return 0x01030000; }
        }

        public List<CMwCmd> Cmds
        {
            get { return ((Chunk004)GetChunk(0x01030004)).Cmds; }
        }

        public List<CBlockVariable> Variables
        {
            get { return ((Chunk004)GetChunk(0x01030004)).Variables; }
        }

        public CBlockVariable GetVariable(string name)
        {
            CBlockVariable varHere = Variables.Where(v => v.Name == name).FirstOrDefault();
            if (varHere != null)
                return varHere;

            if (Block != null)
                return Block.GetVariable(name);

            return null;
        }

        public class Chunk004 : NodeChunk
        {
            public List<CMwCmd> Cmds;
            public List<CBlockVariable> DummyVariables;
            public List<CBlockVariable> Variables;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNodeList(ref Cmds);
                archive.ReadWriteList(ref DummyVariables);
                archive.ReadWriteList(ref Variables);

                if (!archive.Writing)
                {
                    foreach (CMwCmd cmd in Cmds)
                        cmd.Block = (CMwCmdBlock)node;

                    foreach (CBlockVariable v in Variables)
                        v.Block = (CMwCmdBlock)node;
                }
            }
        }

        public override string ToString(int indent)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{{\r\n", GetIndent(indent));
            
            foreach (CBlockVariable variable in Variables)
            {
                sb.AppendFormat("{0}{1} {2};\r\n", GetIndent(indent + 1), variable.TypeName, variable.Name);
            }
            if (Variables.Count > 0)
                sb.Append("\r\n");

            foreach (CMwCmd cmd in Cmds)
            {
                sb.Append(cmd.ToString(indent + 1));
            }

            sb.AppendFormat("{0}}}\r\n", GetIndent(indent));
            return sb.ToString();
        }
    }
}
