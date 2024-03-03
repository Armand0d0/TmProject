using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdBlockCast : CMwCmdBlock
    {
        public override uint ID
        {
            get { return 0x01065000; }
        }

        public class Chunk000 : NodeChunk
        {
            public string String0;
            public uint Dword0;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref String0);
                archive.ReadWrite(ref Dword0);
            }
        }
    }
}
