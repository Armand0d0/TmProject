using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    internal class CSystemFile : CClassicBuffer
    {
        public CSystemFile(string filePath, bool write)
            : base(write ? File.OpenWrite(filePath) : File.OpenRead(filePath), write)
        {

        }
    }
}
