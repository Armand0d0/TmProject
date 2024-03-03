using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdProc : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01097000; }
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
                ParamInterface.Block = value;
                foreach (CMwCmd arg in Arguments)
                    arg.Block = value;

                if (Node0 != null)
                    Node0.Block = value;
            }
        }

        public CMwCmdParamInterface ParamInterface
        {
            get { return ((Chunk002)GetChunk(0x01097002)).ParamInterface; }
            set { ((Chunk002)GetChunk(0x01097002)).ParamInterface = value; }
        }

        public List<CMwCmd> Arguments
        {
            get { return ((Chunk002)GetChunk(0x01097002)).Arguments; }
            set { ((Chunk002)GetChunk(0x01097002)).Arguments = value; }
        }

        public CMwCmd Node0
        {
            get { return ((Chunk002)GetChunk(0x01097002)).Node0; }
            set { ((Chunk002)GetChunk(0x01097002)).Node0 = value; }
        }

        public class Chunk002 : NodeChunk
        {
            public CMwCmdParamInterface ParamInterface;
            public List<CMwCmd> Arguments;
            public CMwCmd Node0;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref ParamInterface, 1);
                archive.ReadWriteNodeList(ref Arguments);
                archive.ReadWriteNode(ref Node0);
            }
        }

        public override string ToString(int indent)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}(", GetIndent(indent), ParamInterface);
            bool first = true;
            foreach (CMwCmd arg in Arguments)
            {
                if (!first)
                    sb.Append(", ");

                sb.Append(arg.ToString());
                first = false;
            }
            sb.Append(");\r\n");
            return sb.ToString();
        }
    }
}
