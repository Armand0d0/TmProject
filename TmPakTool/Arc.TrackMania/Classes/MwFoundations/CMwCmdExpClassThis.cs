using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpClassThis : CMwCmdExpClass
    {
        public override uint ID
        {
            get { return 0x010A2000; }
        }

        public override string ToString(int indent)
        {
            return "this";
        }
    }
}
