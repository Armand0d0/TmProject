using System;
using System.IO;

namespace paktool
{
    class StreamByteProvider : Be.Windows.Forms.IByteProvider
    {
        public StreamByteProvider(Stream stream)
        {
            m_Stream = stream;
        }

        public byte ReadByte(long index)
        {
            if (m_Stream.Position != index)
                m_Stream.Position = index;

            return (byte)m_Stream.ReadByte();
        }

        public void WriteByte(long index, byte value)
        {
            
        }

        public void InsertBytes(long index, byte[] bs)
        {
            
        }

        public void DeleteBytes(long index, long length)
        {
            
        }

        public long Length
        {
            get
            {
                return m_Stream.Length;
            }
        }

        public event EventHandler LengthChanged;

        public bool HasChanges()
        {
            return false;
        }

        public void ApplyChanges()
        {
            
        }

        public event EventHandler Changed;

        public bool SupportsWriteByte()
        {
            return false;
        }

        public bool SupportsInsertBytes()
        {
            return false;
        }

        public bool SupportsDeleteBytes()
        {
            return false;
        }


        private Stream m_Stream;
    }
}
