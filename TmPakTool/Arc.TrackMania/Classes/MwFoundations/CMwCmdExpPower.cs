using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpPower : CMwCmdExpNumBin
    {
        public override uint ID
        {
            get { return 0x0104A000; }
        }

        public override string ToString(int indent)
        {
            return string.Format("Pow({0}, {1})", Value1, Value2);
        }
    }
}
