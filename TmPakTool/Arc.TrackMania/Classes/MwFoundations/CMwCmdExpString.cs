using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpString : CMwCmdExp
    {
        public override uint ID
        {
            get { return 0x0104E000; }
        }

        public string Value
        {
            get { return ((Chunk000)GetChunk(0x0104E000)).Value; }
            set { ((Chunk000)GetChunk(0x0104E000)).Value = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public string Value;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Value);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("\"{0}\"", Value.Replace(@"\", @"\\").Replace("\"", "\\\""));
        }
    }
}
