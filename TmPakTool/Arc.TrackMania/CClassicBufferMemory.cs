using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    internal class CClassicBufferMemory : CClassicBuffer
    {
        public CClassicBufferMemory(bool write)
            : base(new MemoryStream(), write)
        {

        }

        public CClassicBufferMemory(byte[] data, bool write)
            : base(new MemoryStream(data), write)
        {

        }
    }
}
