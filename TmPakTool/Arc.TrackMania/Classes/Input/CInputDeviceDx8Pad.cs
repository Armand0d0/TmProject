using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Input
{
    public class CInputDeviceDx8Pad : CInputDevice
    {
        public override uint ID
        {
            get { return 0x1300C000; }
        }
    }
}
