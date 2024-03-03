using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes;
using Arc.TrackMania.Classes.MwFoundations;

namespace Arc.TrackMania
{
    public class CMwClassInfo
    {
        private int _index;
        private string _name;
        private CMwEngineInfo _engineInfo;
        private Type _classType;
        private Dictionary<int, CMwMemberInfo> _members;

        public CMwClassInfo(string name, Dictionary<int, CMwMemberInfo> members)
        {
            _name = name;
            _members = members;
        }

        public CMwEngineInfo Engine
        {
            get { return _engineInfo; }
            internal set { _engineInfo = value; }
        }

        public Type ClassType
        {
            get
            {
                if(_classType == null)
                    _classType = Type.GetType("Arc.TrackMania.Classes." + Engine.Name + "." + Name);

                return _classType;
            }
        }

        public uint ID
        {
            get { return ((uint)Engine.ID << 24) | ((uint)Index << 12); }
        }

        internal int Index
        {
            get { return _index; }
            set
            {
                _index = value;

                foreach (KeyValuePair<int, CMwMemberInfo> pair in _members)
                {
                    pair.Value.Class = this;
                    pair.Value.Index = pair.Key;
                }
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public CMwMemberInfo GetMemberInfo(uint memberID)
        {
            CMwMemberInfo member;
            _members.TryGetValue((int)(memberID & 0xFFF), out member);
            return member;
        }

        public IEnumerable<CMwFieldInfo> Fields
        {
            get
            {
                return _members.Values.OfType<CMwFieldInfo>();
            }
        }

        public IEnumerable<CMwMethodInfo> Methods
        {
            get
            {
                return _members.Values.OfType<CMwMethodInfo>();
            }
        }

        public CMwNod Instantiate()
        {
            if (ClassType == null)
                return null;

            return (CMwNod)Activator.CreateInstance(ClassType);
        }
    }   
}
