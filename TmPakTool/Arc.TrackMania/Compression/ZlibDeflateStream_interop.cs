using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Arc.TrackMania.Compression
{
    internal class ZlibDeflateStream : Stream
    {
        #region zlib interop

        [StructLayout(LayoutKind.Sequential)]
        private class z_stream
        {
            public IntPtr next_in;
            public int avail_in;
            public int total_in;

            public IntPtr next_out;
            public int avail_out;
            public int total_out;

            public IntPtr msg;
            public IntPtr state;

            public IntPtr zalloc;
            public IntPtr zfree;
            public IntPtr opaque;

            public int data_type;
            public uint adler;
            public uint reserved;
        }

        private enum ZFlushType
        {
            Z_NO_FLUSH = 0,
            Z_PARTIAL_FLUSH = 1,
            Z_SYNC_FLUSH = 2,
            Z_FULL_FLUSH = 3,
            Z_FINISH = 4,
            Z_BLOCK = 5,
            Z_TREES = 6
        }

        private enum ZStatus
        {
            Z_OK = 0,
            Z_STREAM_END = 1,
            Z_NEED_DICT = 2,
            Z_ERRNO = -1,
            Z_STREAM_ERROR = -2,
            Z_DATA_ERROR = -3,
            Z_MEM_ERROR = -4,
            Z_BUF_ERROR = -5,
            Z_VERSION_ERROR = -6
        }

        [DllImport("zlib1.dll")]
        private static extern ZStatus deflateInit_(z_stream stream, int level, string version, int stream_size);

        [DllImport("zlib1.dll")]
        private static extern ZStatus deflate(z_stream stream, ZFlushType flush);

        [DllImport("zlib1.dll")]
        private static extern ZStatus deflateEnd(z_stream stream);

        [DllImport("zlib1.dll")]
        private static extern ZStatus inflateInit_(z_stream stream, string version, int stream_size);

        [DllImport("zlib1.dll")]
        private static extern ZStatus inflate(z_stream stream, ZFlushType flush);

        [DllImport("zlib1.dll")]
        private static extern ZStatus inflateEnd(z_stream stream);

        #endregion

        private const int COMPRESSED_SIZE = 0x100;
        private const int UNCOMPRESSED_SIZE = 0x400;

        private Stream _innerStream;
        private z_stream _zlibStream;
        private byte[] _compressedMem;
        private byte[] _uncompressedMem;
        private IntPtr _zlibCompressedMem;
        private IntPtr _zlibUncompressedMem;

        private bool _compressing;
        private long _position;
        private bool _disposed;

        public ZlibDeflateStream(Stream innerStream)
        {
            _innerStream = innerStream;

            _compressedMem = new byte[COMPRESSED_SIZE];
            _uncompressedMem = new byte[UNCOMPRESSED_SIZE];
            _zlibCompressedMem = Marshal.AllocHGlobal(COMPRESSED_SIZE);
            _zlibUncompressedMem = Marshal.AllocHGlobal(UNCOMPRESSED_SIZE);
            
            _zlibStream = new z_stream();
            _zlibStream.next_in = _zlibCompressedMem;
            _zlibStream.avail_in = 0;
            _zlibStream.next_out = _zlibUncompressedMem;
            _zlibStream.avail_out = UNCOMPRESSED_SIZE;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_compressing)
                {
                    ZStatus status;
                    do
                    {
                        _zlibStream.next_out = _zlibCompressedMem;
                        _zlibStream.avail_out = COMPRESSED_SIZE;
                        status = VerifyStatus(deflate(_zlibStream, ZFlushType.Z_FINISH));
                        int outChunkSize = (int)_zlibStream.next_out - (int)_zlibCompressedMem;
                        Marshal.Copy(_compressedMem, 0, _zlibCompressedMem, outChunkSize);
                        _innerStream.Write(_compressedMem, 0, outChunkSize);
                    } while (status != ZStatus.Z_STREAM_END);
                    VerifyStatus(deflateEnd(_zlibStream));
                }
                else
                {
                    VerifyStatus(inflateEnd(_zlibStream));
                }

                Marshal.FreeHGlobal(_zlibCompressedMem);
                Marshal.FreeHGlobal(_zlibUncompressedMem);
                _zlibStream = null;
                _zlibCompressedMem = IntPtr.Zero;
                _zlibUncompressedMem = IntPtr.Zero;

                if (disposing)
                {
                    _innerStream.Dispose();
                    _innerStream = null;
                }

                _disposed = true;
            }
            base.Dispose(disposing);
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get
            {
                return _position;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (Position == 0)
            {
                _compressing = false;
                VerifyStatus(inflateInit_(_zlibStream, "1.2.5", Marshal.SizeOf(_zlibStream)));
            }

            if (_compressing)
                throw new NotSupportedException();

            int left = count;
            while (left > 0)
            {
                if (_zlibStream.avail_in == 0)
                {
                    _zlibStream.next_in = _zlibCompressedMem;
                    _zlibStream.avail_in = Math.Min((int)(_innerStream.Length - _innerStream.Position), COMPRESSED_SIZE);
                    _innerStream.Read(_compressedMem, 0, _zlibStream.avail_in);
                    Marshal.Copy(_compressedMem, 0, _zlibCompressedMem, _zlibStream.avail_in);
                }
                do
                {
                    _zlibStream.next_out = _zlibUncompressedMem;
                    _zlibStream.avail_out = Math.Min(UNCOMPRESSED_SIZE, left);
                    VerifyStatus(inflate(_zlibStream, ZFlushType.Z_SYNC_FLUSH));
                    int outChunkSize = (int)_zlibStream.next_out - (int)_zlibUncompressedMem;
                    Marshal.Copy(_zlibUncompressedMem, buffer, offset, outChunkSize);
                    offset += outChunkSize;
                    left -= outChunkSize;
                } while (left > 0 && _zlibStream.avail_in > 0);
            }
            _position += count;
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (Position == 0)
            {
                _compressing = true;
                VerifyStatus(deflateInit_(_zlibStream, 8, "1.2.5", Marshal.SizeOf(_zlibStream)));
            }

            if (!_compressing)
                throw new NotSupportedException();

            int left = count;
            while (left > 0)
            {
                if (_zlibStream.avail_in == 0)
                {
                    _zlibStream.next_in = _zlibUncompressedMem;
                    _zlibStream.avail_in = Math.Min(UNCOMPRESSED_SIZE, left);
                    Marshal.Copy(buffer, offset, _zlibStream.next_in, _zlibStream.avail_in);
                    offset += _zlibStream.avail_in;
                    left -= _zlibStream.avail_in;
                }
                do
                {
                    _zlibStream.next_out = _zlibCompressedMem;
                    _zlibStream.avail_out = COMPRESSED_SIZE;
                    VerifyStatus(deflate(_zlibStream, ZFlushType.Z_SYNC_FLUSH));
                    int outChunkSize = (int)_zlibStream.next_out - (int)_zlibCompressedMem;
                    Marshal.Copy(_zlibCompressedMem, _compressedMem, 0, outChunkSize);
                    _innerStream.Write(_compressedMem, 0, outChunkSize);
                } while (_zlibStream.avail_in > 0);
            }
            _position += count;
        }

        private static ZStatus VerifyStatus(ZStatus status)
        {
            switch (status)
            {
                case ZStatus.Z_BUF_ERROR:
                    throw new Exception("Buffer error");

                case ZStatus.Z_DATA_ERROR:
                    throw new Exception("Data error");

                case ZStatus.Z_ERRNO:
                    throw new Exception("Filesystem error");

                case ZStatus.Z_MEM_ERROR:
                    throw new Exception("Memory error");

                case ZStatus.Z_STREAM_ERROR:
                    throw new Exception("Stream error");

                case ZStatus.Z_VERSION_ERROR:
                    throw new Exception("Version error");
            }
            return status;
        }
    }
}
