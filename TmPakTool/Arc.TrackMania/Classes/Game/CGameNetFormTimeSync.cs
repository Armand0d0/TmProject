using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Game
{
    public class CGameNetFormTimeSync : Arc.TrackMania.Classes.Net.CNetFormTimed
    {
        public override uint ID
        {
            get { return 0x03069000; }
        }
    }
}
