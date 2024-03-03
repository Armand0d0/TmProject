using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Game
{
    public class CGameMasterServer : Arc.TrackMania.Classes.Net.CNetMasterServer
    {
        public override uint ID
        {
            get { return 0x03006000; }
        }
    }
}
