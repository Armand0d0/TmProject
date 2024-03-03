using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CBlockVariable : Arc.TrackMania.Classes.MwFoundations.CMwNod
    {
        public enum VarType
        {
            UInt32,
            Int32,
            Float,
            String,
            Class,
            WeakClass,
            Vec2D,
            Vec3D,
            Iso4,
            Unknown
        }

        public CMwCmdBlock Block;
        public string Name;
        public VarType Type;
        public uint ClassID;

        public override uint ID
        {
            get { return 0x01080000; }
        }

        public string TypeName
        {
            get
            {
                switch (Type)
                {
                    case VarType.UInt32:
                        return "uint";

                    case VarType.Int32:
                        return "int";

                    case VarType.Float:
                        return "float";

                    case VarType.String:
                        return "string";

                    case VarType.Class:
                    case VarType.WeakClass:
                        return CMwEngineManager.GetClassInfo(ClassID).Name;

                    case VarType.Vec2D:
                        return "vec2d";

                    case VarType.Vec3D:
                        return "vec3d";

                    case VarType.Iso4:
                        return "iso4";
                }
                return "";
            }
        }

        internal override void ReadWrite(CClassicArchive archive)
        {
            archive.ReadWrite(ref Name);
            int ignored = 0;
            archive.ReadWrite(ref ignored);

            uint type = (uint)Type;
            archive.ReadWrite(ref type);
            Type = (VarType)type;

            if (Type == VarType.Class || Type == VarType.WeakClass)
                archive.ReadWrite(ref ClassID);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
