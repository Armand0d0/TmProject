using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    [TypeDescriptionProvider(typeof(FieldTypeDescriptionProvider<FileReference>))]
    public class FileReference
    {
        private byte _version;
        private string _file;

        public FileReference()
        {

        }

        public FileReference(string file)
        {
            _version = 2;
            _file = file;
        }

        public string File
        {
            get { return _file; }
            set { _file = value; }
        }

        internal void ReadWrite(CClassicArchive archive)
        {
            archive.ReadWrite(ref _version);
            archive.ReadWrite(ref _file);
            if (_file.Length > 0)
            {
                string s2 = "";
                archive.ReadWrite(ref s2);
            }
        }
    }
}
