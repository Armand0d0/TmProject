using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpClass : CMwCmdExp
    {
        public override uint ID
        {
            get { return 0x01056000; }
        }

        public uint ClassID
        {
            get { return ((Chunk000)GetChunk(0x01056000)).ClassID; }
            set { ((Chunk000)GetChunk(0x01056000)).ClassID = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public uint ClassID;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref ClassID);
            }
        }

        public override string ToString(int indent)
        {
            return "null";
        }
    }
}
