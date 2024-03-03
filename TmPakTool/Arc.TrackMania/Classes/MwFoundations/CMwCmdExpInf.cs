using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpInf : CMwCmdExpBoolBin
    {
        public override uint ID
        {
            get { return 0x01041000; }
        }

        public override string ToString(int indent)
        {
            return BinOpToString("<", Value1, Value2);
        }
    }
}
