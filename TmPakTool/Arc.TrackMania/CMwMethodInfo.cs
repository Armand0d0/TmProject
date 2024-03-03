using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    public class CMwMethodInfo : CMwMemberInfo
    {
        private uint[] _argTypeIDs;
        private string[] _argNames;

        internal CMwMethodInfo(string name, uint retTypeID, uint[] argTypeIDs, string[] argNames)
            : base(name, retTypeID)
        {
            _argTypeIDs = argTypeIDs;
            _argNames = argNames;
        }

        public int ArgCount
        {
            get
            {
                if (_argTypeIDs == null)
                    return 0;

                return _argTypeIDs.Length;
            }
        }

        public bool IsArgSimple(int index)
        {
            return IsTypeIDSimple(_argTypeIDs[index]);
        }

        public SimpleType GetArgSimpleType(int index)
        {
            return (SimpleType)_argTypeIDs[index];
        }

        public CMwClassInfo GetArgClassType(int index)
        {
            return CMwEngineManager.GetClassInfo(_argTypeIDs[index]);
        }

        public string GetArgName(int index)
        {
            return _argNames[index];
        }
    }
}
