using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Input
{
    public class CInputDeviceDx8Keyboard : CInputDevice
    {
        public override uint ID
        {
            get { return 0x1300B000; }
        }
    }
}
