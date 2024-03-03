using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmd : CMwNod
    {
        private static string[] _vecFields = { "length", "x", "y", "z" };
        private static string[] _iso4Fields = { "_11", "_12", "_13",
                                                "_21", "_22", "_23",
                                                "_31", "_32", "_33",
                                                "_41", "_42", "_43" };
        private static List<string> _indents = new List<string>();

        protected CMwCmdBlock _block;

        public override uint ID
        {
            get { return 0x01005000; }
        }

        public virtual CMwCmdBlock Block
        {
            get { return _block; }
            set { _block = value; }
        }

        public override string ToString()
        {
            return ToString(0);
        }

        public virtual string ToString(int indent)
        {
            return base.ToString();
        }

        protected static string GetNumField(CMwCmdExp exp, int index)
        {
            if (exp is CMwCmdExpVec2 || exp is CMwCmdExpVec3)
            {
                return GetVectorField(index);
            }
            else if (exp is CMwCmdExpIso4)
            {
                return GetIso4Field(index);
            }
            else
            {
                return "";
            }
        }

        protected static string GetNumField(CBlockVariable.VarType type, int index)
        {
            if (type == CBlockVariable.VarType.Vec2D || type == CBlockVariable.VarType.Vec3D)
            {
                return GetVectorField(index);
            }
            else if (type == CBlockVariable.VarType.Iso4)
            {
                return GetIso4Field(index);
            }
            else
            {
                return "";
            }
        }

        protected static string GetVectorField(int index)
        {
            return "." + _vecFields[1 + index];
        }

        protected static string GetIso4Field(int index)
        {
            return "." + _iso4Fields[index];
        }

        protected static string GetIndent(int indent)
        {
            if (indent < 0)
                throw new ArgumentOutOfRangeException("index");

            while (_indents.Count <= indent)
                _indents.Add(new string(' ', 4 * _indents.Count));

            return _indents[indent];
        }
    }
}
