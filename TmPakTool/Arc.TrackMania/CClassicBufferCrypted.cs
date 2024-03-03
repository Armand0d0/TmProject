using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Simias.Encryption;

namespace Arc.TrackMania
{
    internal class CClassicBufferCrypted : CClassicBuffer
    {
        private CClassicBuffer _innerBuffer;
        private uint _size;

        public enum Algorithm
        {
            Xor,
            BlowfishCBC,
            BlowfishCTR
        }

        public CClassicBufferCrypted(CClassicBuffer innerBuffer, Algorithm algo, byte[] key)
            : base(GetCryptStream(innerBuffer.Stream, algo, key, innerBuffer.Writing), innerBuffer.Writing)
        {
            _innerBuffer = innerBuffer;
            _size = innerBuffer.Size;
        }

        public CClassicBufferCrypted(CClassicBuffer innerBuffer, Algorithm algo, byte[] key, uint size)
            : this(innerBuffer, algo, key)
        {
            _size = size;
        }

        public override void Initialize(byte[] data, uint offset, uint count)
        {
            if (_stream is BlowfishCBCCryptStream)
                ((BlowfishCBCCryptStream)_stream).Initialize(data, offset, count);
        }

        public override void FinishWriting()
        {
            if (_stream is BlowfishCBCCryptStream)
                ((BlowfishCBCCryptStream)_stream).FinishWriting();
        }

        public override uint Size
        {
            get
            {
                return _innerBuffer.Writing ? (uint)_stream.Length : _size;
            }
        }

        private static Stream GetCryptStream(Stream innerStream, Algorithm algo, byte[] key, bool writing)
        {
            switch (algo)
            {
                case Algorithm.Xor:
                    return new XorCryptStream(innerStream, key);

                case Algorithm.BlowfishCBC:
                    BlowfishCBCCryptStream stream = new BlowfishCBCCryptStream(innerStream, key, writing);
                    if (writing)
                    {
                        innerStream.Write(BitConverter.GetBytes(stream.IV), 0, 8);
                    }
                    else
                    {
                        byte[] iv = new byte[8];
                        innerStream.Read(iv, 0, 8);
                        stream.IV = BitConverter.ToUInt64(iv, 0);
                    }
                    return stream;
            }
            throw new NotSupportedException("Unsupported cryptographic algorithm requested");
        }

        private abstract class CryptStreamBase : Stream
        {
            protected Stream _innerStream;
            protected byte[] _key;

            public CryptStreamBase(Stream innerStream, byte[] key)
            {
                _innerStream = innerStream;
                _key = key;
            }

            public override bool CanRead
            {
                get { return _innerStream.CanRead; }
            }

            public override bool CanSeek
            {
                get { return _innerStream.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return _innerStream.CanWrite; }
            }

            public override void Flush()
            {
                _innerStream.Flush();
            }

            public override long Length
            {
                get { return _innerStream.Length; }
            }

            public override long Position
            {
                get { return _innerStream.Position; }
                set { _innerStream.Position = value; }
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _innerStream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }
        }

        private class XorCryptStream : CryptStreamBase
        {
            public XorCryptStream(Stream innerStream, byte[] key)
                : base(innerStream, key)
            {
                
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                int result = _innerStream.Read(buffer, offset, count);
                for (int i = offset; i < offset + count; i++)
                {
                    buffer[i] ^= GetKeyStreamByte(i);
                }
                return result;
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                byte[] toWrite = new byte[count];
                Array.Copy(buffer, offset, toWrite, 0, count);
                for (int i = 0; i < count; i++)
                {
                    toWrite[i] ^= GetKeyStreamByte(i);
                }
                _innerStream.Write(toWrite, 0, count);
            }

            private byte GetKeyStreamByte(int pos)
            {
                return rol(_key[pos % 16], (pos / 17) % 8);
            }

            private static byte rol(byte input, int amount)
            {
                return (byte)((input << amount) | (input >> (8 - amount)));
            }
        }

        private class BlowfishCBCCryptStream : CryptStreamBase
        {
            private bool _writing;
            private Blowfish _blowfish;
            private ulong _iv;
            private ulong _ivXor;
            private byte[] _buffer;
            private uint _bufferIndex;
            private uint _totalIndex;

            public BlowfishCBCCryptStream(Stream innerStream, byte[] key, bool writing)
                : base(innerStream, key)
            {
                _writing = writing;
                _blowfish = new Blowfish(key);
                _buffer = new byte[8];
                _bufferIndex = 0;
                _totalIndex = 0;
            }

            public ulong IV
            {
                get { return _iv; }
                set { _iv = value; }
            }

            public override long Position
            {
                get { return _totalIndex; }
                set { throw new NotSupportedException(); }
            }

            public override long Length
            {
                get { return _writing ? Position : _innerStream.Length; }
            }

            public override bool CanSeek
            {
                get { return false; }
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public void Initialize(byte[] data, uint offset, uint count)
            {
                for (int i = 0; i < count; i++)
                {
                    uint lopart = (uint)(_ivXor & 0xFFFFFFFF);
                    uint hipart = (uint)(_ivXor >> 32);
                    lopart = (uint)(data[offset + i] | 0xAA) ^ (uint)((lopart << 13) | (hipart >> 19));
                    hipart = (uint)((_ivXor << 13) >> 32);
                    _ivXor = ((ulong)hipart << 32) | lopart;
                }
            }

            public void FinishWriting()
            {
                byte[] buf = new byte[] { 0 };
                while (_bufferIndex % 8 != 0)
                {
                    Write(buf, 0, 1);
                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (_writing)
                    throw new NotSupportedException();

                if (_totalIndex == 0)
                {
                    _iv ^= _ivXor;
                    _ivXor = 0;
                }

                for (int i = 0; i < count; i++)
                {
                    if (_bufferIndex % 8 == 0)
                    {
                        if (_bufferIndex == 0x100)
                        {
                            _iv ^= _ivXor;
                            _ivXor = 0;
                            _bufferIndex = 0;
                        }

                        _innerStream.Read(_buffer, 0, 8);
                        ulong nextIV = BitConverter.ToUInt64(_buffer, 0);
                        _blowfish.Decipher(_buffer, 8);
                        ulong block = BitConverter.ToUInt64(_buffer, 0);
                        block ^= _iv;
                        _buffer = BitConverter.GetBytes(block);
                        _iv = nextIV;
                    }
                    buffer[offset + i] = _buffer[_bufferIndex % 8];
                    _bufferIndex++;
                    _totalIndex++;
                }
                return count;
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                if (!_writing)
                    throw new NotSupportedException();

                if (_totalIndex == 0)
                {
                    _iv ^= _ivXor;
                    _ivXor = 0;
                }

                for (int i = 0; i < count; i++)
                {
                    _buffer[_bufferIndex % 8] = buffer[offset + i];
                    _bufferIndex++;
                    _totalIndex++;

                    if (_bufferIndex % 8 == 0)
                    {
                        ulong block = BitConverter.ToUInt64(_buffer, 0);
                        block ^= _iv;
                        _buffer = BitConverter.GetBytes(block);

                        _blowfish.Encipher(_buffer, 8);
                        _innerStream.Write(_buffer, 0, 8);
                        _iv = BitConverter.ToUInt64(_buffer, 0);

                        if (_bufferIndex == 0x100)
                        {
                            _iv ^= _ivXor;
                            _ivXor = 0;
                            _bufferIndex = 0;
                        }
                    }
                }
            }

        }

        private class BlowfishCTRCryptStream : CryptStreamBase
        {
            public BlowfishCTRCryptStream(Stream innerStream, byte[] key)
                : base(new BlowfishStream(innerStream, new Blowfish(key), BlowfishStream.Target.Encrypted), key)
            {

            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }
        }
    }
}
