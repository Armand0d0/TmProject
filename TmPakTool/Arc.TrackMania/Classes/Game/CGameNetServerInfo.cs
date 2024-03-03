using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Game
{
    public class CGameNetServerInfo : Arc.TrackMania.Classes.Net.CNetMasterHost
    {
        public override uint ID
        {
            get { return 0x0302E000; }
        }
    }
}
