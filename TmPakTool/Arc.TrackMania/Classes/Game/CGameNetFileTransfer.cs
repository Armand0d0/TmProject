using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Game
{
    public class CGameNetFileTransfer : Arc.TrackMania.Classes.Net.CNetFileTransfer
    {
        public override uint ID
        {
            get { return 0x03068000; }
        }
    }
}
