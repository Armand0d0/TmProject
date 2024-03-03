using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Plug
{
    public class CPlugFileJpg : CPlugFileImg
    {
        private ImageSource _imgSource;

        public override uint ID
        {
            get { return 0x09022000; }
        }
    }
}
