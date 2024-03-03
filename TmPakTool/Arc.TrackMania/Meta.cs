using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    [TypeDescriptionProvider(typeof(FieldTypeDescriptionProvider<Meta>))]
    public class Meta
    {
        public string Field1;
        public string Field2;
        public string Author;

        public Meta()
        {

        }

        public Meta(string field1, string field2, string author)
        {
            Field1 = field1;
            Field2 = field2;
            Author = author;
        }

        internal void ReadWrite(CClassicArchive archive)
        {
            archive.ReadWriteLookbackString(ref Field1);
            archive.ReadWriteLookbackString(ref Field2);
            archive.ReadWriteLookbackString(ref Author);
        }
    }
}
