using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdParamInterface : IReadWriteEx
    {
        private CMwCmdBlock _block;
        private uint _useGlobalInstance = 1;

        public CMwCmdBlock Block
        {
            get { return _block; }
            set { _block = value; }
        }

        public bool UseGlobalInstance
        {
            get { return _useGlobalInstance != 0; }
            set { _useGlobalInstance = value ? 1u : 0u; }
        }

        public string VarName;
        public CMwNod Instance;
        public List<CMwCmd> IndexExprs;
        public List<CMwStack> FixedIndexStacks;

        internal override void ReadWrite(CClassicArchive archive, int version)
        {
            if (version == 0)
            {
                archive.ReadWrite(ref _useGlobalInstance);
                if (_useGlobalInstance == 0)
                    archive.ReadWrite(ref VarName);

                archive.ReadWriteNodeList(ref IndexExprs);
                archive.ReadWriteList(ref FixedIndexStacks);
            }
            else if (version == 1)
            {
                uint ignored = 0;
                archive.ReadWrite(ref ignored);

                archive.ReadWrite(ref _useGlobalInstance);
                if (_useGlobalInstance == 0)
                    archive.ReadWrite(ref VarName);

                archive.ReadWriteNode(ref Instance);
                archive.ReadWriteNodeList(ref IndexExprs);
                archive.ReadWriteList(ref FixedIndexStacks);
            }
            else
            {
                throw new NotSupportedException(string.Format("CMwCmdParamInterface: version {0} is unrecognized",
                    version));
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (UseGlobalInstance)
                sb.Append("this");
            else if (!string.IsNullOrEmpty(VarName))
                sb.Append(VarName);

            for (int i = 0; i < FixedIndexStacks.Count; i++)
            {
                CMwStack stack = FixedIndexStacks[i];
                for (int j = stack.Count - 1; j >= 0; j--)
                {
                    if (j == 0 && i < IndexExprs.Count && IndexExprs[i] != null)
                    {
                        sb.AppendFormat("[{0}]", IndexExprs[i]);
                    }
                    else
                    {
                        sb.Append(stack[j].ToString());
                    }
                }
            }

            return sb.ToString();
        }
    }
}
