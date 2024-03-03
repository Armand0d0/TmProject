using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.NadeoPak
{
    public class NadeoPak : NadeoPakFolderBase
    {
        private string _filePath;
        private PackList _packList;
        private byte[] _key;

        private byte[] _md5 = new byte[16];
        private uint _metaDataOffset;
        private uint _dataOffset;
        private uint _metaDataUncompressedSize;
        private uint _metaDataCompressedSize;
        private byte[] _dqword0 = new byte[16];
        private uint _flags;

        /// <summary>
        /// Creates a new NadeoPak from a file.
        /// </summary>
        /// <param name="filePath">The path to the .pak file.</param>
        public NadeoPak(string filePath)
            : base(null, Path.GetFileNameWithoutExtension(filePath))
        {
            _pak = this;
            _packList = new PackList(Path.Combine(Path.GetDirectoryName(filePath), "packlist.dat"));
            _key = _packList.GetPakKey(Path.GetFileNameWithoutExtension(filePath));
            if (_key == null)
                throw new Exception(string.Format("Could not find decryption key for {0} in packlist.dat", filePath));

            _filePath = filePath;
            using (Stream stream = File.OpenRead(filePath))
            {
                ReadWrite(new CClassicBuffer(stream, false));
            }
        }

        /// <summary>
        /// Reassembles the .pak contents and writes it back to the original file.
        /// </summary>
        public void Save()
        {
            Save(_filePath);
        }

        /// <summary>
        /// Reassembles the .pak contents and writes it to the specified file.
        /// </summary>
        /// <param name="filePath">The file path to save to.</param>
        public void Save(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Truncate, FileAccess.Write))
            {
                ReadWrite(new CClassicBuffer(stream, true));
            }
        }

        internal uint FirstFileOffset
        {
            get { return _dataOffset; }
        }

        /// <summary>
        /// Gets the PackList that was used to retrieve the decryption key for this NadeoPak.
        /// </summary>
        public PackList PackList
        {
            get { return _packList; }
        }

        /// <summary>
        /// Gets the Blowfish decryption key for this NadeoPak.
        /// </summary>
        public byte[] Key
        {
            get { return _key; }
        }

        private void ReadWrite(CClassicBuffer buffer)
        {
            buffer.ReadWrite("NadeoPak");

            uint version = 3;
            buffer.ReadWrite(ref version);
            if (version != 3)
                throw new NotSupportedException("Only version 3 NadeoPaks are supported");

            byte[] dataForWrite = null;
            byte[] fileMetasForWrite = null;
            if (buffer.Writing)
            {
                CClassicBuffer dataBuffer = new CClassicBufferMemory(true);
                ReadWriteData(dataBuffer);
                dataForWrite = dataBuffer.Bytes;

                CClassicBuffer fileMetaBuffer = new CClassicBufferMemory(true);
                ReadWriteFileMetas(fileMetaBuffer);
                _metaDataUncompressedSize = fileMetaBuffer.Size;

                CClassicBuffer encryptedFileMetaBuffer = new CClassicBufferMemory(true);
                CClassicBuffer compressedFileMetaBuffer = new CClassicBufferCrypted(encryptedFileMetaBuffer,
                    CClassicBufferCrypted.Algorithm.BlowfishCBC, _key);
                fileMetaBuffer = new CClassicBufferZlib(compressedFileMetaBuffer, _metaDataUncompressedSize);
                ReadWriteFileMetas(fileMetaBuffer);
                _metaDataCompressedSize = compressedFileMetaBuffer.Size;
                compressedFileMetaBuffer.FinishWriting();
                while (compressedFileMetaBuffer.Size % 0x100 != 0)
                    compressedFileMetaBuffer.Write((byte)0);
                fileMetasForWrite = encryptedFileMetaBuffer.Bytes;
            }

            if (buffer.Writing)
                UpdateHeader((uint)dataForWrite.Length);

            CClassicBufferCrypted headerBuffer = new CClassicBufferCrypted(buffer,
                CClassicBufferCrypted.Algorithm.BlowfishCBC, _key);
            ReadWriteHeader(headerBuffer);

            if (buffer.Writing)
            {
                buffer.Write(dataForWrite);
                buffer.Write(fileMetasForWrite);
            }
            else
            {
                ReadWriteData(buffer);

                buffer.Position = _metaDataOffset;
                buffer = new CClassicBufferCrypted(buffer, CClassicBufferCrypted.Algorithm.BlowfishCBC, _key);
                buffer = new CClassicBufferZlib(buffer, _metaDataUncompressedSize);
                ReadWriteFileMetas(buffer);
            }
        }

        private void UpdateHeader(uint dataSize)
        {
            CClassicBufferMemory buffer = new CClassicBufferMemory(true);
            _md5 = new byte[16];

            ReadWriteHeader(buffer);
            byte[] header = buffer.Bytes;

            _dataOffset = buffer.Size;
            if (_dataOffset % 8 != 0)
                _dataOffset = (_dataOffset & ~7u) + 8;

            _dataOffset = 0x14 + _dataOffset;
            Array.Copy(BitConverter.GetBytes(_dataOffset), 0, header, 0x14, 4);

            _metaDataOffset = _dataOffset + dataSize;
            Array.Copy(BitConverter.GetBytes(_metaDataOffset), 0, header, 0x10, 4);

            _md5 = new MD5CryptoServiceProvider().ComputeHash(header);
        }

        private void ReadWriteHeader(CClassicBuffer buffer)
        {
            uint startPos = buffer.Position;

            buffer.ReadWrite(_md5);
            buffer.ReadWrite(ref _metaDataOffset);
            buffer.ReadWrite(ref _dataOffset);
            buffer.ReadWrite(ref _metaDataUncompressedSize);
            buffer.ReadWrite(ref _metaDataCompressedSize);
            buffer.ReadWrite(_dqword0);
            buffer.ReadWrite(ref _flags);

            List<NadeoPakFolder> allFolders = ReadWriteFolders(buffer);
            ReadWriteFileHeaders(buffer, allFolders);

            if (buffer.Writing)
                buffer.FinishWriting();
        }

        private List<NadeoPakFolder> ReadWriteFolders(CClassicBuffer buffer)
        {
            List<NadeoPakFolder> allFolders;
            if (buffer.Writing)
                allFolders = AllFolders.ToList();
            else
                allFolders = new List<NadeoPakFolder>();

            int numFolders = allFolders.Count;
            buffer.ReadWrite(ref numFolders);
            if (numFolders < 0)
                throw new Exception("Negative number of folders");

            for (int i = 0; i < numFolders; i++)
            {
                if (!buffer.Writing)
                    allFolders.Add(new NadeoPakFolder(this));

                allFolders[i].ReadWriteHeader(buffer);
            }

            if (allFolders.Count > 2 && allFolders[2].Name.Length > 4)
            {
                byte[] nameBytes = Encoding.Unicode.GetBytes(allFolders[2].Name);
                buffer.Initialize(nameBytes, 4, 4);
            }

            if (!buffer.Writing)
            {
                foreach (NadeoPakFolder folder in allFolders)
                    folder.ResolveParentFolder(allFolders);
            }

            return allFolders;
        }

        private void ReadWriteFileHeaders(CClassicBuffer buffer, List<NadeoPakFolder> allFolders)
        {
            List<NadeoPakFile> allFiles;
            if (buffer.Writing)
                allFiles = AllFiles.ToList();
            else
                allFiles = new List<NadeoPakFile>();

            int numFiles = allFiles.Count;
            buffer.ReadWrite(ref numFiles);
            if (numFiles < 0)
                throw new Exception("Negative number of files");

            for (int i = 0; i < numFiles; i++)
            {
                if (!buffer.Writing)
                    allFiles.Add(new NadeoPakFile(this));

                allFiles[i].ReadWriteHeader(buffer, allFolders);
            }
        }

        private void ReadWriteData(CClassicBuffer buffer)
        {
            if (buffer.Writing)
                _dataOffset = buffer.Position;

            foreach (NadeoPakFile file in AllFiles)
            {
                file.ReadWriteData(buffer);
            }
        }

        private void ReadWriteFileMetas(CClassicBuffer buffer)
        {
            if (buffer.Writing)
            {
                int index = 0;
                foreach (NadeoPakFile file in AllFiles)
                {
                    if (file.MetaData != null)
                    {
                        buffer.Write(index);
                        buffer.Write(file.MetaData);
                    }
                    index++;
                }
                buffer.Write(-1);
                buffer.FinishWriting();
            }
            else
            {
                List<NadeoPakFile> allFiles = AllFiles.ToList();

                while (true)
                {
                    int index = buffer.ReadInt32();
                    if (index < 0 || index >= allFiles.Count)
                        break;

                    // The file metadata is a dataless gbx file. Read it as raw bytes for
                    // now since we can't parse all of the chunks yet
                    //GameBox.GameBox gbx = new GameBox.GameBox();
                    //gbx.ReadWrite(buffer, true);
                    //allFiles[index].Metadata = gbx;

                    byte[] upToHeaderSize = buffer.ReadBytes(0xD);
                    uint headerSize = buffer.ReadUInt32();
                    uint totalHeaderSize = (uint)upToHeaderSize.Length + 4 + headerSize + 4;
                    
                    byte[] metadata = new byte[totalHeaderSize];
                    Array.Copy(upToHeaderSize, 0, metadata, 0, upToHeaderSize.Length);
                    Array.Copy(BitConverter.GetBytes(headerSize), 0, metadata, upToHeaderSize.Length, 4);
                    Array.Copy(buffer.ReadBytes(headerSize + 4), 0, metadata, upToHeaderSize.Length + 4, headerSize + 4);
                    allFiles[index].MetaData = metadata;
                }
            }
        }
    }
}
