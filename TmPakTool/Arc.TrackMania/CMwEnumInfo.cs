using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    public class CMwEnumInfo : CMwFieldInfo
    {
        private string[] _members;

        internal CMwEnumInfo(string name, string[] members)
            : base(name, (uint)SimpleType.Enum)
        {
            _members = members;
        }

        public int Count
        {
            get { return _members.Length; }
        }

        public string this[int index]
        {
            get { return _members[index]; }
        }
    }
}
