using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdBlockProcedure : CMwCmdBlock
    {
        public override uint ID
        {
            get { return 0x01098000; }
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
                foreach (CMwCmd node in Nodes)
                    node.Block = value;
            }
        }

        public List<CMwCmd> Nodes
        {
            get { return ((Chunk000)GetChunk(0x01098000)).Nodes; }
            set { ((Chunk000)GetChunk(0x01098000)).Nodes = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public List<CMwCmd> Nodes;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNodeList(ref Nodes);
            }
        }
    }
}
