using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Xml
{
    public class CHdrDocument : CXmlDocument
    {
        public override uint ID
        {
            get { return 0x1400B000; }
        }
    }
}
