using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    public abstract class IReadWrite
    {
        internal abstract void ReadWrite(CClassicArchive archive);
    }
}
