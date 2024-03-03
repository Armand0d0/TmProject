using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Plug
{
    public class CPlugFileGPU : CPlugFileText
    {
        public override uint ID
        {
            get { return 0x09040000; }
        }
    }
}
