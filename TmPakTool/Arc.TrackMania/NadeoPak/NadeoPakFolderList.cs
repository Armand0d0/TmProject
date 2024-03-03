using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.NadeoPak
{
    public class NadeoPakFolderList : IList<NadeoPakFolder>
    {
        private NadeoPakFolderBase _parentFolder;
        private List<NadeoPakFolder> _folders = new List<NadeoPakFolder>();

        internal NadeoPakFolderList(NadeoPakFolderBase parentFolder)
        {
            _parentFolder = parentFolder;
        }

        public int IndexOf(NadeoPakFolder item)
        {
            return _folders.IndexOf(item);
        }

        public void Insert(int index, NadeoPakFolder item)
        {
            if (!_folders.Contains(item))
                item.ParentFolder = _parentFolder;
        }

        public void RemoveAt(int index)
        {
            this[index].ParentFolder = null;
        }

        public NadeoPakFolder this[int index]
        {
            get
            {
                return _folders[index];
            }
            set
            {
                this[index].ParentFolder = null;
                value.ParentFolder = _parentFolder;
            }
        }

        public NadeoPakFolder this[string name]
        {
            get
            {
                return _folders.Where(f => f.Name == name).FirstOrDefault();
            }
        }

        public void Add(NadeoPakFolder item)
        {
            item.ParentFolder = _parentFolder;
        }

        public void AddRange(IEnumerable<NadeoPakFolder> items)
        {
            foreach (NadeoPakFolder folder in items)
                Add(folder);
        }

        internal void InternalAdd(NadeoPakFolder item)
        {
            _folders.Add(item);
            _parentFolder.OnFolderAdded(item);
        }

        public void Clear()
        {
            while (_folders.Count > 0)
                _folders[0].ParentFolder = null;
        }

        public bool Contains(NadeoPakFolder item)
        {
            return _folders.Contains(item);
        }

        public void CopyTo(NadeoPakFolder[] array, int arrayIndex)
        {
            _folders.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _folders.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(NadeoPakFolder item)
        {
            if (!Contains(item))
                return false;

            item.ParentFolder = null;
            return true;
        }

        public void RemoveRange(IEnumerable<NadeoPakFolder> items)
        {
            foreach (NadeoPakFolder folder in items)
                Remove(folder);
        }

        internal void InternalRemove(NadeoPakFolder item)
        {
            _folders.Remove(item);
            _parentFolder.OnFolderRemoved(item);
        }

        public IEnumerator<NadeoPakFolder> GetEnumerator()
        {
            return _folders.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _folders.GetEnumerator();
        }
    }
}
