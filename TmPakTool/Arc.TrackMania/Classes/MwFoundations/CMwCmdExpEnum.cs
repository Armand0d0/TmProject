using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpEnum : CMwCmdExp
    {
        public override uint ID
        {
            get { return 0x01059000; }
        }

        public int Value
        {
            get { return ((Chunk000)GetChunk(0x01059000)).Value; }
            set { ((Chunk000)GetChunk(0x01059000)).Value = value; }
        }

        public uint EnumID
        {
            get { return ((Chunk000)GetChunk(0x01059000)).EnumID; }
            set { ((Chunk000)GetChunk(0x01059000)).EnumID = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public int Value;
            public uint EnumID;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref Value);
                archive.ReadWrite(ref EnumID);
            }
        }

        public override string ToString(int indent)
        {
            CMwEnumInfo enumInfo = (CMwEnumInfo)CMwEngineManager.GetMemberInfo(EnumID);
            return string.Format("{0}.{1}[\"{2}\"]", enumInfo.Class.Name, enumInfo.Name, enumInfo[Value]);
        }
    }
}
