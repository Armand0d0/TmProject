using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Game
{
    public class CGameNetFormGameSync : Arc.TrackMania.Classes.Net.CNetNod
    {
        public override uint ID
        {
            get { return 0x0300A000; }
        }
    }
}
