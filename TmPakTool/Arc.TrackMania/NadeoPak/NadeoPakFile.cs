using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes.MwFoundations;

namespace Arc.TrackMania.NadeoPak
{
    public class NadeoPakFile : INotifyPropertyChanged
    {
        private NadeoPak _pak;
        private NadeoPakFolderBase _folder;
        private string _name;
        private uint _dword0;
        private uint _uncompressedSize;
        private uint _compressedSize;
        private uint _offset;
        private uint _classID;
        private ulong _flags;
        private byte[] _encryptedData;
        private CClassicBuffer _encryptedWriteBuffer;
        private CClassicBuffer _compressedWriteBuffer;
        private CClassicBuffer _plainWriteBuffer;

        private byte[] _metadata;

        /// <summary>
        /// Creates a new NadeoPakFile.
        /// </summary>
        /// <param name="pak">The NadeoPak which the file belongs to.</param>
        public NadeoPakFile(NadeoPak pak)
        {
            _pak = pak;
        }

        /// <summary>
        /// Creates a new NadeoPakFile.
        /// </summary>
        /// <param name="pak">The NadeoPak which the file belongs to.</param>
        /// <param name="folder">The folder containing the file.</param>
        /// <param name="name">The file name.</param>
        /// <param name="classID">The class ID. If 0, the instance will try to determine the class ID from the file extension.</param>
        /// <param name="flags">The 64-bit file flags.</param>
        /// <param name="data">The plain file data. null means an empty file.</param>
        public NadeoPakFile(NadeoPak pak, NadeoPakFolderBase folder, string name, uint classID, ulong flags, byte[] data)
            : this(pak)
        {
            Name = name;
            Flags = flags;
            Data = data;

            ClassID = classID;
            if (ClassID == 0)
            {
                string[] parts = name.Split('.');
                string extension = "";
                for (int i = parts.Length - 1; i > 0; i--)
                {
                    extension = "." + parts[i] + extension;
                    classID = ClassIDByExtension.ExtensionToClassID(extension);
                    if (classID != 0)
                        ClassID = classID;
                }

                if (classID == 0)
                    ClassID = 0x09020000;
            }

            Folder = folder;
        }

        /// <summary>
        /// Gets or sets the folder which this file is in. Changing the folder will remove the file
        /// from its old folder's Files collection.
        /// </summary>
        public NadeoPakFolderBase Folder
        {
            get { return _folder; }
            set
            {
                if (value == _folder)
                    return;

                if (_folder != null)
                    _folder.Files.InternalRemove(this);

                _folder = value;

                if (_folder != null)
                    _folder.Files.InternalAdd(this);

                OnPropertyChanged("Folder");
            }
        }

        /// <summary>
        /// Gets or sets the file's name.
        /// </summary>
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

        /// <summary>
        /// Gets the full path of the file within the NadeoPak.
        /// </summary>
        public string FullPath
        {
            get
            {
                string result = Name;
                NadeoPakFolderBase folder = Folder;
                while (folder != null && folder is NadeoPakFolder)
                {
                    result = folder.Name + result;
                    folder = ((NadeoPakFolder)folder).ParentFolder;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets or sets the file's class ID.
        /// </summary>
        public uint ClassID
        {
            get { return _classID; }
            set
            {
                if (value == _classID)
                    return;

                _classID = value;
                OnPropertyChanged("ClassID");
            }
        }

        /// <summary>
        /// Gets the name of the class corresponding to the ClassID.
        /// </summary>
        public string ClassName
        {
            get { return CMwEngineManager.GetClassName(_classID); }
        }

        /// <summary>
        /// Gets or sets the file's flags.
        /// </summary>
        public ulong Flags
        {
            get { return _flags; }
            set
            {
                if (value == _flags)
                    return;

                _flags = value;
                OnPropertyChanged("Flags");
            }
        }

        /// <summary>
        /// Gets or sets the metadata attached to the file.
        /// </summary>
        public byte[] MetaData
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        /// <summary>
        /// Gets or sets the unencrypted, uncompressed content of the file.
        /// </summary>
        public byte[] Data
        {
            get
            {
                FlushWriteBuffer();
                return GetBuffer(false).ReadBytes(_uncompressedSize);
            }
            set
            {
                GetBuffer(true).Write(value ?? new byte[] { });
                FlushWriteBuffer();
                OnPropertyChanged("Data");
            }
        }

        /// <summary>
        /// Gets whether or not the file is compressed in the archive (this depends on the flags).
        /// </summary>
        public bool Compressed
        {
            get { return (_flags & 0x7C) != 0; }
        }

        /// <summary>
        /// Gets the uncompressed size of the file.
        /// </summary>
        public uint Size
        {
            get { return _uncompressedSize; }
        }

        /// <summary>
        /// Creates a node for the file's class ID.
        /// </summary>
        /// <returns></returns>
        public CMwNod CreateClassInstance()
        {
            return CMwEngineManager.CreateClassInstance(_classID);
        }

        internal void ReadWriteHeader(CClassicBuffer buffer, List<NadeoPakFolder> allFolders)
        {
            int folderIndex = -1;
            if (buffer.Writing && (_folder as NadeoPakFolder) != null)
                folderIndex = allFolders.IndexOf(_folder as NadeoPakFolder);

            buffer.ReadWrite(ref folderIndex);
            if (folderIndex < 0)
                Folder = _pak;
            else if (folderIndex < allFolders.Count)
                Folder = allFolders[folderIndex];
            else
                Folder = null;

            buffer.ReadWrite(ref _name);
            buffer.ReadWrite(ref _dword0);
            buffer.ReadWrite(ref _uncompressedSize);
            buffer.ReadWrite(ref _compressedSize);
            buffer.ReadWrite(ref _offset);
            buffer.ReadWrite(ref _classID);
            buffer.ReadWrite(ref _flags);

            if (!buffer.Writing && _name.Contains("\\"))
            {
                string[] pathParts = _name.Split('\\');
                _name = pathParts[pathParts.Length - 1];
                for (int i = 0; i < pathParts.Length - 1; i++)
                {
                    string nextFolderName = pathParts[i] + "\\";
                    NadeoPakFolder nextFolder = Folder.Folders[nextFolderName];
                    if (nextFolder == null)
                        nextFolder = new NadeoPakFolder(_pak, Folder, nextFolderName);

                    Folder = nextFolder;
                }
            }
        }

        internal void ReadWriteData(CClassicBuffer buffer)
        {
            if (buffer.Writing)
            {
                _offset = buffer.Position - _pak.FirstFileOffset;

                FlushWriteBuffer();
                buffer.Write(_encryptedData);
            }
            else
            {
                buffer.Position = _pak.FirstFileOffset + _offset;
                uint roundedDataSize = 8 + _compressedSize;
                if ((roundedDataSize & 7u) != 0)
                    roundedDataSize = (roundedDataSize & ~7u) + 8;

                _encryptedData = buffer.ReadBytes(roundedDataSize);
            }
        }

        internal CClassicBuffer GetBuffer(bool forWriting)
        {
            CClassicBuffer encryptedBuffer = forWriting ? new CClassicBufferMemory(true) :
                new CClassicBufferMemory(_encryptedData, false);
            CClassicBuffer compressedBuffer = new CClassicBufferCrypted(encryptedBuffer,
                CClassicBufferCrypted.Algorithm.BlowfishCBC, _pak.Key, _compressedSize);
            CClassicBuffer plainBuffer;
            if (Compressed)
                plainBuffer = new CClassicBufferZlib(compressedBuffer, forWriting ? 0 : _uncompressedSize);
            else
                plainBuffer = compressedBuffer;

            if (forWriting)
            {
                _encryptedWriteBuffer = encryptedBuffer;
                _compressedWriteBuffer = compressedBuffer;
                _plainWriteBuffer = plainBuffer;
            }

            return plainBuffer;
        }

        private void FlushWriteBuffer()
        {
            if (_encryptedWriteBuffer == null)
                return;

            _uncompressedSize = _plainWriteBuffer.Size;
            _plainWriteBuffer.FinishWriting();

            _compressedSize = _compressedWriteBuffer.Size;
            _compressedWriteBuffer.FinishWriting();

            _encryptedData = _encryptedWriteBuffer.Bytes;

            _plainWriteBuffer = null;
            _compressedWriteBuffer = null;
            _encryptedWriteBuffer = null;
        }

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
