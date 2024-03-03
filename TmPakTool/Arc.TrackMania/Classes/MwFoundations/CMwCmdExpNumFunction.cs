using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    public class CMwCmdExpNumFunction : CMwCmdExpNum
    {
        public override uint ID
        {
            get { return 0x01055000; }
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
                if (Function != null)
                    Function.Block = value;

                if (FuncInterface != null)
                    FuncInterface.Block = value;
            }
        }

        public CMwCmdExpNum Function
        {
            get { return ((Chunk000)GetChunk(0x01055000)).Function; }
            set { ((Chunk000)GetChunk(0x01055000)).Function = value; }
        }

        public CMwCmdFunctionInterface FuncInterface
        {
            get { return ((Chunk000)GetChunk(0x01055000)).FuncInterface; }
            set { ((Chunk000)GetChunk(0x01055000)).FuncInterface = value; }
        }

        public class Chunk01046000 : NodeChunk
        {
            private uint _isFloat;

            public bool IsFloat
            {
                get { return _isFloat != 0; }
                set { _isFloat = value ? 1u : 0u; }
            }

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref _isFloat);
            }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpNum Function;
            public uint Version;
            public CMwCmdFunctionInterface FuncInterface;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref Function);
                archive.ReadWrite(ref Version);
                if (Version == 11)
                    archive.ReadWrite(ref FuncInterface);
            }
        }

        public override string ToString(int indent)
        {
            if (Function != null)
                return Function.ToString();
            else
                return FuncInterface.ToString();
        }
    }
}
