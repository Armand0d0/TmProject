using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Input
{
    public class CInputPortNull : CInputPort
    {
        public override uint ID
        {
            get { return 0x13003000; }
        }
    }
}
