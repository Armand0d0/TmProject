using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.NadeoPak
{
    public class NadeoPakFileList : IList<NadeoPakFile>
    {
        private NadeoPakFolderBase _folder;
        private List<NadeoPakFile> _files = new List<NadeoPakFile>();

        internal NadeoPakFileList(NadeoPakFolderBase folder)
        {
            _folder = folder;
        }

        public int IndexOf(NadeoPakFile item)
        {
            return _files.IndexOf(item);
        }

        public void Insert(int index, NadeoPakFile item)
        {
            if (!_files.Contains(item))
                item.Folder = _folder;
        }

        public void RemoveAt(int index)
        {
            this[index].Folder = null;
        }

        public NadeoPakFile this[int index]
        {
            get
            {
                return _files[index];
            }
            set
            {
                this[index].Folder = null;
                value.Folder = _folder;
            }
        }

        public NadeoPakFile this[string name]
        {
            get
            {
                return _files.Where(f => f.Name == name).FirstOrDefault();
            }
        }

        public void Add(NadeoPakFile item)
        {
            item.Folder = _folder;
        }

        public void AddRange(IEnumerable<NadeoPakFile> items)
        {
            foreach (NadeoPakFile file in items)
                Add(file);
        }

        internal void InternalAdd(NadeoPakFile item)
        {
            _files.Add(item);
            _folder.OnFileAdded(item);
        }

        public void Clear()
        {
            while (_files.Count > 0)
                _files[0].Folder = null;
        }

        public bool Contains(NadeoPakFile item)
        {
            return _files.Contains(item);
        }

        public void CopyTo(NadeoPakFile[] array, int arrayIndex)
        {
            _files.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _files.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(NadeoPakFile item)
        {
            if (!Contains(item))
                return false;

            item.Folder = null;
            return true;
        }

        public void RemoveRange(IEnumerable<NadeoPakFile> items)
        {
            foreach (NadeoPakFile file in items)
                Remove(file);
        }

        internal void InternalRemove(NadeoPakFile item)
        {
            _files.Remove(item);
            _folder.OnFileRemoved(item);
        }

        public IEnumerator<NadeoPakFile> GetEnumerator()
        {
            return _files.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _files.GetEnumerator();
        }
    }
}
