using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdIdentInterface : IReadWrite
    {
        private CMwCmdBlock _block;
        public string VarName;

        public CMwCmdBlock Block
        {
            get { return _block; }
            set { _block = value; }
        }

        public CBlockVariable Variable
        {
            get { return _block.GetVariable(VarName); }
        }

        internal override void ReadWrite(CClassicArchive archive)
        {
            archive.ReadWrite(ref VarName);
        }

        public override string ToString()
        {
            return VarName;
        }
    }
}
