using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    public abstract class IReadWriteEx
    {
        internal abstract void ReadWrite(CClassicArchive archive, int version);
    }
}
