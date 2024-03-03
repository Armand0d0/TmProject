using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    public class CMwStack : IReadWrite
    {
        public enum ItemType
        {
            MemberID,
            NumIndex,
            StringIndex
        }

        public class StackItem
        {
            public ItemType Type;

            public uint MemberID;
            public uint NumIndex;
            public string StringIndex;

            public StackItem(ItemType type)
            {
                Type = type;
            }

            internal void ReadWrite(CClassicArchive archive)
            {
                switch (Type)
                {
                    case ItemType.MemberID:
                        archive.ReadWrite(ref MemberID);
                        break;

                    case ItemType.NumIndex:
                        archive.ReadWrite(ref NumIndex);
                        break;

                    case ItemType.StringIndex:
                        uint ignored = 0;
                        archive.ReadWrite(ref ignored);
                        archive.ReadWrite(ref StringIndex);
                        break;
                }
            }

            public override string ToString()
            {
                switch (Type)
                {
                    case ItemType.MemberID:
                        return string.Format(".{0}", CMwEngineManager.GetMemberInfo(MemberID).Name);

                    case ItemType.NumIndex:
                        return string.Format("[{0}]", NumIndex);

                    case ItemType.StringIndex:
                        return string.Format("[\"{0}\"]", StringIndex);
                }
                return base.ToString();
            }
        }

        private List<StackItem> _items = new List<StackItem>();

        public int Count
        {
            get { return _items.Count; }
        }

        public StackItem this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        public void Push(StackItem item)
        {
            _items.Add(item);
        }

        public StackItem Pop()
        {
            if (Count == 0)
                throw new Exception("Attempt to pop from an empty stack");

            StackItem item = _items[_items.Count - 1];
            _items.RemoveAt(_items.Count - 1);
            return item;
        }

        public StackItem Peek()
        {
            if (Count == 0)
                throw new Exception("Attempt to peek from an empty stack");

            return _items[_items.Count - 1];
        }

        public void Clear()
        {
            _items.Clear();
        }

        internal override void ReadWrite(CClassicArchive archive)
        {
            List<uint> types;
            if (archive.Writing)
            {
                types = _items.Select(i => (uint)i.Type).ToList();
                archive.Write(types);
            }
            else
            {
                types = archive.ReadUInt32List();
                _items = types.Select(type => new StackItem((ItemType)type)).ToList();
            }

            foreach (StackItem item in _items)
                item.ReadWrite(archive);
        }
    }
}
