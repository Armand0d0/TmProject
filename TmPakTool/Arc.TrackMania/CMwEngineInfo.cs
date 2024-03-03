using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    public class CMwEngineInfo
    {
        private int _id;
        private string _name;
        private Dictionary<int, CMwClassInfo> _classes;

        public CMwEngineInfo(string name, Dictionary<int, CMwClassInfo> classes)
        {
            _name = name;
            _classes = classes;

            foreach (KeyValuePair<int, CMwClassInfo> pair in _classes)
            {
                pair.Value.Engine = this;
                pair.Value.Index = pair.Key;
            }
        }

        public int ID
        {
            get { return _id; }
            internal set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<CMwClassInfo> Classes
        {
            get { return _classes.Values; }
        }

        public CMwClassInfo GetClassInfo(uint classID)
        {
            CMwClassInfo classInfo;
            _classes.TryGetValue((int)((classID >> 12) & 0xFFF), out classInfo);
            return classInfo;
        }
    }
}
