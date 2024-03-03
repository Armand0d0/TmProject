using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdFunctionInterface : IReadWrite
    {
        private CMwCmdBlock _block;

        public CMwCmd Function;
        public List<CMwCmd> Arguments;

        public CMwCmdBlock Block
        {
            get { return _block; }
            set
            {
                _block = value;
                if (Arguments != null)
                {
                    foreach (CMwCmd arg in Arguments)
                    {
                        arg.Block = value;
                    }
                }
            }
        }

        internal override void ReadWrite(CClassicArchive archive)
        {
            uint flag = 0;
            archive.ReadWrite(ref flag);
            if (flag != 0)
                return;

            archive.ReadWriteNode(ref Function);
            archive.ReadWriteNodeList(ref Arguments);
        }

        public override string ToString()
        {
            if (Function == null)
                return "";

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}(", Function);
            if (Arguments != null)
            {
                bool first = true;
                foreach (CMwCmd arg in Arguments)
                {
                    if (!first)
                        sb.Append(", ");

                    sb.AppendFormat("{0}", arg);
                    first = false;
                }
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
