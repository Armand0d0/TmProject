using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.NadeoPak
{
    public class NadeoPakFolderBase : INotifyPropertyChanged
    {
        protected string _name;
        protected NadeoPak _pak;
        protected NadeoPakFolderList _folders;
        protected NadeoPakFileList _files;

        public NadeoPakFolderBase(NadeoPak pak)
        {
            _pak = pak;
            _folders = new NadeoPakFolderList(this);
            _files = new NadeoPakFileList(this);
        }

        public NadeoPakFolderBase(NadeoPak pak, string name)
            : this(pak)
        {
            _name = name;
        }

        public event Action<NadeoPakFolder> FolderAdded;
        public event Action<NadeoPakFolder> FolderRemoved;
        public event Action<NadeoPakFile> FileAdded;
        public event Action<NadeoPakFile> FileRemoved;

        public NadeoPak Pack
        {
            get { return _pak; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;

                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public IEnumerable<NadeoPakFolder> AllFolders
        {
            get
            {
                Stack<NadeoPakFolder> dfs = new Stack<NadeoPakFolder>();
                for (int i = _folders.Count - 1; i >= 0; i--)
                    dfs.Push(_folders[i]);

                while (dfs.Count > 0)
                {
                    NadeoPakFolder folder = dfs.Pop();
                    yield return folder;

                    for (int i = folder.Folders.Count - 1; i >= 0; i--)
                        dfs.Push(folder.Folders[i]);
                }
            }
        }

        public IEnumerable<NadeoPakFile> AllFiles
        {
            get
            {
                foreach (NadeoPakFile file in Files)
                {
                    yield return file;
                }

                foreach (NadeoPakFolder folder in AllFolders)
                {
                    foreach (NadeoPakFile file in folder.Files)
                        yield return file;
                }
            }
        }

        public NadeoPakFolderList Folders
        {
            get { return _folders; }
        }

        public NadeoPakFileList Files
        {
            get { return _files; }
        }

        public NadeoPakFolder GetOrCreateFolder(string path)
        {
            NadeoPakFolderBase folder = this;
            foreach (string part in path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string nextFolderName = part + "\\";
                if (folder.Folders[nextFolderName] != null)
                    folder = folder.Folders[nextFolderName];
                else
                    folder = new NadeoPakFolder(Pack, folder, nextFolderName);
            }
            return folder as NadeoPakFolder;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void OnFolderAdded(NadeoPakFolder folder)
        {
            if (FolderAdded != null)
                FolderAdded(folder);
        }

        internal void OnFolderRemoved(NadeoPakFolder folder)
        {
            if (FolderRemoved != null)
                FolderRemoved(folder);
        }

        internal void OnFileAdded(NadeoPakFile file)
        {
            if (FileAdded != null)
                FileAdded(file);
        }

        internal void OnFileRemoved(NadeoPakFile file)
        {
            if (FileRemoved != null)
                FileRemoved(file);
        }
    }
}
