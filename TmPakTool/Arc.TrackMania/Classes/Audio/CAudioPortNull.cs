using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Audio
{
    public class CAudioPortNull : CAudioPort
    {
        public override uint ID
        {
            get { return 0x10008000; }
        }
    }
}
