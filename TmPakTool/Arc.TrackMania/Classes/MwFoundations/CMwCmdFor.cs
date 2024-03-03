using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdFor : CMwCmdInst
    {
        public override uint ID
        {
            get { return 0x01035000; }
        }

        public override CMwCmdBlock Block
        {
            get
            {
                return base.Block;
            }
            set
            {
                base.Block = value;
                Min.Block = value;
                Max.Block = value;
                Body.Block = value;
            }
        }

        public string VarName
        {
            get { return ((Chunk000)GetChunk(0x01035000)).VarName; }
            set { ((Chunk000)GetChunk(0x01035000)).VarName = value; }
        }

        public CMwCmdExpNum Min
        {
            get { return ((Chunk000)GetChunk(0x01035000)).Min; }
            set { ((Chunk000)GetChunk(0x01035000)).Min = value; }
        }

        public CMwCmdExpNum Max
        {
            get { return ((Chunk000)GetChunk(0x01035000)).Max; }
            set { ((Chunk000)GetChunk(0x01035000)).Max = value; }
        }

        public CMwCmdBlock Body
        {
            get { return ((Chunk000)GetChunk(0x01035000)).Body; }
            set { ((Chunk000)GetChunk(0x01035000)).Body = value; }
        }

        public class Chunk000 : NodeChunk
        {
            public string VarName;
            public CMwCmdExpNum Min;
            public CMwCmdExpNum Max;
            public CMwCmdBlock Body;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref VarName);
                archive.ReadWriteNode(ref Min);
                archive.ReadWriteNode(ref Max);
                archive.ReadWriteNode(ref Body);
            }
        }

        public override string ToString(int indent)
        {
            return string.Format("{0}for ({1} = {2} ... {3})\r\n{4}", GetIndent(indent),
                VarName, Min, Max, Body.ToString(indent));
        }
    }
}
