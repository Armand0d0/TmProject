using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.Xml
{
    public class CHdrComment : CXmlComment
    {
        public override uint ID
        {
            get { return 0x14009000; }
        }
    }
}
