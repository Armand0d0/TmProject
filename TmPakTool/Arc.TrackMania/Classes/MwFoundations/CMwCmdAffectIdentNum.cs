using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdAffectIdentNum : CMwCmdAffectIdent
    {
        public override uint ID
        {
            get { return 0x01074000; }
        }

        public int Index
        {
            get { return ((Chunk000)GetChunk(0x01074000)).Index; }
            set { ((Chunk000)GetChunk(0x01074000)).Index = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public int Index = -1;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Index);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("{0}{1}{2} = {3};\r\n", GetIndent(indent), IdentInterface.VarName,
                GetNumField(IdentInterface.Variable.Type, Index), Value);
        }
    }
}
