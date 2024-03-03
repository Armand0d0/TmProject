using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Classes.MwFoundations
{
    /// <summary>
    /// Finds the first occurrence of a search string in an input string, then
    /// returns the input string reduced to everything left or right of this found
    /// searching string, including or excluding the search string itself.
    /// </summary>
    public class CMwCmdExpStringTrunc : CMwCmdExpString
    {
        public override uint ID
        {
            get { return 0x010A3000; }
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
                InputString.Block = value;
                SearchString.Block = value;
                IncludeSearchString.Block = value;
            }
        }

        public CMwCmdExpString InputString
        {
            get { return ((Chunk000)GetChunk(0x010A3000)).InputString; }
            set { ((Chunk000)GetChunk(0x010A3000)).InputString = value; }
        }

        public CMwCmdExpString SearchString
        {
            get { return ((Chunk000)GetChunk(0x010A3000)).SearchString; }
            set { ((Chunk000)GetChunk(0x010A3000)).SearchString = value; }
        }

        public CMwCmdExpBool IncludeSearchString
        {
            get { return ((Chunk000)GetChunk(0x010A3000)).IncludeSearchString; }
            set { ((Chunk000)GetChunk(0x010A3000)).IncludeSearchString = value; }
        }

        public bool Left
        {
            get { return ((Chunk000)GetChunk(0x010A3000)).Left; }
            set { ((Chunk000)GetChunk(0x010A3000)).Left = value; }
        }

        public class Chunk0104E000 : NodeChunk
        {
            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                
            }
        }

        public class Chunk000 : NodeChunk
        {
            public CMwCmdExpString InputString;
            public CMwCmdExpString SearchString;
            public CMwCmdExpBool IncludeSearchString;
            private uint _left;

            public bool Left
            {
                get { return _left != 0; }
                set { _left = value ? 1u : 0u; }
            }

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWriteNode(ref InputString);
                archive.ReadWriteNode(ref SearchString);
                archive.ReadWriteNode(ref IncludeSearchString);
                archive.ReadWrite(ref _left);
            }
        }

        public override string ToString(int indent)
        {
            if (Left)
                return string.Format("{0}.Left({1}, {2})", InputString, SearchString, IncludeSearchString);
            else
                return string.Format("{0}.Right({1}, {2})", InputString, SearchString, IncludeSearchString);
        }
    }
}
