using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.NadeoPak
{
    public class NadeoPakFolder : NadeoPakFolderBase
    {
        private int _parentFolderIndex;
        private NadeoPakFolderBase _parentFolder;

        /// <summary>
        /// Creates a new NadeoPakFolder.
        /// </summary>
        /// <param name="pak">The NadeoPak which the folder belongs to.</param>
        public NadeoPakFolder(NadeoPak pak)
            : base(pak)
        {
            
        }

        /// <summary>
        /// Creates a new NadeoPakFolder.
        /// </summary>
        /// <param name="pak">The NadeoPak which the folder belongs to.</param>
        /// <param name="parentFolder">The parent folder.</param>
        /// <param name="name">The folder name.</param>
        public NadeoPakFolder(NadeoPak pak, NadeoPakFolderBase parentFolder, string name)
            : this(pak)
        {
            Name = name;
            ParentFolder = parentFolder;
        }

        /// <summary>
        /// Gets the full path of the folder within the NadeoPak hierarchy.
        /// </summary>
        public string FullPath
        {
            get
            {
                string result = Name;
                NadeoPakFolderBase folder = ParentFolder;
                while (folder != null && folder is NadeoPakFolder)
                {
                    result = folder.Name + result;
                    folder = ((NadeoPakFolder)folder).ParentFolder;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets or sets the folder's parent folder. Changing the parent folder will remove the folder
        /// from its parent's Folders collection.
        /// </summary>
        public NadeoPakFolderBase ParentFolder
        {
            get { return _parentFolder; }
            set
            {
                if (value == _parentFolder)
                    return;

                if (_parentFolder != null)
                    _parentFolder.Folders.InternalRemove(this);

                _parentFolder = value;

                if (_parentFolder != null)
                    _parentFolder.Folders.InternalAdd(this);

                OnPropertyChanged("ParentFolder");
            }
        }

        internal void ReadWriteHeader(CClassicBuffer buffer)
        {
            if (buffer.Writing)
            {
                if (_parentFolder == null)
                    _parentFolderIndex = -1;
                else
                    _parentFolderIndex = _pak.AllFolders.Cast<NadeoPakFolderBase>().ToList().IndexOf(_parentFolder);
            }
            buffer.ReadWrite(ref _parentFolderIndex);
            buffer.ReadWrite(ref _name);
        }

        internal void ResolveParentFolder(List<NadeoPakFolder> allFolders)
        {
            if (_parentFolderIndex < 0)
                ParentFolder = _pak;
            else if (_parentFolderIndex < allFolders.Count)
                ParentFolder = allFolders[_parentFolderIndex];
            else
                ParentFolder = null;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
