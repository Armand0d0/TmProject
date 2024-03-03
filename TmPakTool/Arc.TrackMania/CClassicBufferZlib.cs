using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Arc.TrackMania
{
    internal class CClassicBufferZlib : CClassicBuffer
    {
        private CClassicBuffer _innerBuffer;
        private uint _uncompressedSize;

        public CClassicBufferZlib(CClassicBuffer innerBuffer, uint uncompressedSize)
            : base(new Compression.ZlibDeflateStream(innerBuffer.Stream, innerBuffer.Writing), innerBuffer.Writing)
        {
            _innerBuffer = innerBuffer;
            _uncompressedSize = uncompressedSize;
        }

        public CClassicBuffer InnerBuffer
        {
            get { return _innerBuffer; }
        }

        public override uint Size
        {
            get { return _uncompressedSize; }
        }

        public override void Initialize(byte[] data, uint offset, uint count)
        {
            _innerBuffer.Initialize(data, offset, count);
        }

        public override void Write(byte[] buf, uint offset, uint count)
        {
            base.Write(buf, offset, count);
            _uncompressedSize += count;
        }

        public override void FinishWriting()
        {
            if (_stream == null)
                return;

            _stream.Dispose();
            _stream = null;
        }
    }
}
