using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Arc.TrackMania.Classes.MwFoundations
{
    [TypeDescriptionProvider(typeof(FieldTypeDescriptionProvider<NodeChunk>))]
    public abstract class NodeChunk
    {
        private CMwNod _node;
        private uint _chunkID;

        public uint ID
        {
            get
            {
                if (_chunkID != 0)
                    return _chunkID;

                Match match = Regex.Match(GetType().Name, @"^Chunk(\w{8})$");
                if (match.Success)
                {
                    _chunkID = uint.Parse(match.Groups[1].Value, NumberStyles.AllowHexSpecifier);
                }
                else
                {
                    uint classID = CMwEngineManager.GetClassInfo(GetType().DeclaringType).ID;
                    uint chunkIndex = uint.Parse(Regex.Match(GetType().Name, @"^Chunk(\w{3})$").Groups[1].Value,
                        NumberStyles.AllowHexSpecifier);
                    _chunkID = classID | chunkIndex;
                }
                return _chunkID;
            }
        }

        public string Name
        {
            get { return CMwEngineManager.GetChunkName(ID); }
        }

        public CMwNod Node
        {
            get { return _node; }
            internal set { _node = value; }
        }

        public virtual bool IsHeaderChunk { get { return false; } }
        public virtual bool IsDetailHeaderChunk { get { return false; } }

        internal abstract void ReadWrite(CClassicArchive archive, CMwNod node);
    }
}
