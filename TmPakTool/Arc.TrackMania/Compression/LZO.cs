using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Arc.TrackMania.Compression
{
    internal class LZO
    {
        [DllImport("lzo.dll")]
        private static extern void __lzo_init3();

        [DllImport("lzo.dll")]
        private static extern void lzo1x_1_compress(byte[] src, uint srclen, byte[] dst, ref uint dstlen, byte[] wrkmem);

        [DllImport("lzo.dll")]
        private static extern void lzo1x_decompress(byte[] src, uint srclen, byte[] dst, ref uint dstlen, byte[] wrkmem);

        private static byte[] _wrkmem = new byte[64 * 1024];

        static LZO()
        {
            __lzo_init3();
        }

        public static byte[] Compress(byte[] data)
        {
            byte[] dst = new byte[data.Length + data.Length / 64 + 16 + 3 + 4];
            uint dstlen = 0;
            lzo1x_1_compress(data, (uint)data.Length, dst, ref dstlen, _wrkmem);
            byte[] ret = new byte[dstlen];
            Array.Copy(dst, 0, ret, 0, dstlen);
            return ret;
        }

        public static byte[] Decompress(byte[] data, uint origLen)
        {
            byte[] dst = new byte[origLen];
            uint dstlen = origLen;
            lzo1x_decompress(data, (uint)data.Length, dst, ref dstlen, _wrkmem);
            return dst;
        }
    }
}
