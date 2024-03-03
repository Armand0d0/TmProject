using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    public class CMwMemberInfo
    {
        public enum SimpleType
        {
            Action,
            Bool,
            BoolArray,
            BoolBuffer,
            BoolBufferCat,
            Class,
            ClassArray,
            ClassBuffer,
            ClassBufferCat,
            Color,
            ColorArray,
            ColorBuffer,
            ColorBufferCat,
            Enum,
            Int,
            IntArray,
            IntBuffer,
            IntBufferCat,
            IntRange,
            Iso4,
            Iso4Array,
            Iso4Buffer,
            Iso4BufferCat,
            Iso3,
            Iso3Array,
            Iso3Buffer,
            Iso3BufferCat,
            MwId,
            MwIdArray,
            MwIdBuffer,
            MwIdBufferCat,
            Natural,
            NaturalArray,
            NaturalBuffer,
            NaturalBufferCat,
            NaturalRange,
            Real,
            RealArray,
            RealBuffer,
            RealBufferCat,
            RealRange,
            String,
            StringArray,
            StringBuffer,
            StringBufferCat,
            StringInt,
            StringIntArray,
            StringIntBuffer,
            StringIntBufferCat,
            Vec2,
            Vec2Array,
            Vec2Buffer,
            Vec2BufferCat,
            Vec3,
            Vec3Array,
            Vec3Buffer,
            Vec3BufferCat,
            Vec4,
            Vec4Array,
            Vec4Buffer,
            Vec4BufferCat,
            Quat,
            QuatArray,
            QuatBuffer,
            QuatBufferCat,
            Proc,
            RefBuffer
        }

        private uint _id;
        private CMwClassInfo _classInfo;
        private uint _typeID;
        private string _name;

        internal CMwMemberInfo(string name, uint typeID)
        {
            _typeID = typeID;
            _name = name;
        }

        public CMwClassInfo Class
        {
            get { return _classInfo; }
            internal set { _classInfo = value; }
        }

        public uint ID
        {
            get { return _id; }
        }

        internal int Index
        {
            set { _id = Class.ID | (uint)value; }
        }

        public string Name
        {
            get { return _name; }
        }

        public bool HasSimpleType
        {
            get { return IsTypeIDSimple(_typeID); }
        }

        public SimpleType GetSimpleType()
        {
            if (!IsTypeIDSimple(_typeID))
                throw new Exception("Member does not have a simple type. Check the HasSimpleType property before calling this method.");

            return (SimpleType)_typeID;
        }

        public CMwClassInfo GetClassType()
        {
            if (IsTypeIDSimple(_typeID))
                throw new Exception("Member does not have a class type. Check the HasSimpleType property before calling this method.");

            return CMwEngineManager.GetClassInfo(_typeID);
        }

        public string TypeName
        {
            get
            {
                if (HasSimpleType)
                    return GetSimpleType().ToString();
                else
                    return GetClassType().Name;
            }
        }

        protected static bool IsTypeIDSimple(uint typeID)
        {
            return typeID < 0x01000000u;
        }
    }
}
