using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Plug
{
    public class CPlugFileAvi : CPlugFileVideo
    {
        public override uint ID
        {
            get { return 0x09032000; }
        }
    }
}
