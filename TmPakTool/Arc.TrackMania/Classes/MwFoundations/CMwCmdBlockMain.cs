using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdBlockMain : CMwCmdBlock
    {
        public override uint ID
        {
            get { return 0x01067000; }
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
                foreach (CMwCmd node in Nodes1)
                    node.Block = value;
                foreach (CMwCmd node in Nodes2)
                    node.Block = value;
                foreach (CMwCmd node in Nodes3)
                    node.Block = value;
            }
        }

        public string Name
        {
            get { return ((Chunk005)GetChunk(0x01067005)).Name; }
            set { ((Chunk005)GetChunk(0x01067005)).Name = value; }
        }

        public uint TargetClassID
        {
            get { return ((Chunk005)GetChunk(0x01067005)).TargetClassID; }
            set { ((Chunk005)GetChunk(0x01067005)).TargetClassID = value; }
        }

        public List<CMwCmd> Nodes1
        {
            get { return ((Chunk005)GetChunk(0x01067005)).Nodes1; }
            set { ((Chunk005)GetChunk(0x01067005)).Nodes1 = value; }
        }

        public List<CMwCmd> Nodes2
        {
            get { return ((Chunk005)GetChunk(0x01067005)).Nodes2; }
            set { ((Chunk005)GetChunk(0x01067005)).Nodes2 = value; }
        }

        public List<CMwCmd> Nodes3
        {
            get { return ((Chunk005)GetChunk(0x01067005)).Nodes3; }
            set { ((Chunk005)GetChunk(0x01067005)).Nodes3 = value; }
        }

        public class Chunk005 : NodeChunk
        {
            public string Name;
            public uint TargetClassID;
            public List<CMwCmd> Nodes1;
            public List<CMwCmd> Nodes2;
            public List<CMwCmd> Nodes3;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteLookbackString(ref Name);
                archive.ReadWrite(ref TargetClassID);
                archive.ReadWriteNodeList(ref Nodes1);
                archive.ReadWriteNodeList(ref Nodes2);
                archive.ReadWriteNodeList(ref Nodes3);
            }
        }

        public override string ToString(int indent)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}class {1}\r\n{0}{{\r\n{2}void main()\r\n", GetIndent(indent),
                CMwEngineManager.GetClassInfo(TargetClassID).Name, GetIndent(indent + 1));
            sb.Append(base.ToString(indent + 1));
            sb.AppendFormat("{0}}}\r\n", GetIndent(indent));
            return sb.ToString();
        }
    }
}
