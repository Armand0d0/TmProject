using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpIso4Ident : CMwCmdExpIso4
    {
        public override uint ID
        {
            get { return 0x01063000; }
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
                IdentInterface.Block = value;
            }
        }

        public CMwCmdIdentInterface IdentInterface
        {
            get { return ((Chunk000)GetChunk(0x01063000)).IdentInterface; }
            set { ((Chunk000)GetChunk(0x01063000)).IdentInterface = value; }
        }

        public class Chunk01062000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdIdentInterface IdentInterface;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref IdentInterface);
            }
        }

        public override string ToString(int indent)
        {
            return IdentInterface.ToString();
        }
    }
}
