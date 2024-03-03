using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    internal class ImageSource
    {
        private byte[] _bytes;
        private ImageFormat _format;

        public ImageSource(byte[] bytes, ImageFormat format)
        {
            _bytes = bytes;
        }

        public byte[] Bytes
        {
            get { return _bytes; }
            set { _bytes = value; }
        }

        public Image Image
        {
            get
            {
                return System.Drawing.Image.FromStream(new MemoryStream(_bytes, false));
            }
            set
            {
                MemoryStream stream = new MemoryStream();
                value.Save(stream, _format);
                stream.Position = 0;
                _bytes = new byte[stream.Length];
                stream.Read(_bytes, 0, _bytes.Length);
            }
        }
    }
}
