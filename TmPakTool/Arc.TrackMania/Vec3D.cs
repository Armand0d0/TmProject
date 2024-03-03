using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    [TypeDescriptionProvider(typeof(FieldTypeDescriptionProvider<Vec3D>))]
    public class Vec3D
    {
        public float X;
        public float Y;
        public float Z;

        internal void ReadWrite(CClassicArchive archive)
        {
            archive.ReadWrite(ref X);
            archive.ReadWrite(ref Y);
            archive.ReadWrite(ref Z);
        }
    }
}
