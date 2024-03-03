using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Plug
{
    public class CPlugFileTga : CPlugFileImg
    {
        public override uint ID
        {
            get { return 0x09023000; }
        }
    }
}
