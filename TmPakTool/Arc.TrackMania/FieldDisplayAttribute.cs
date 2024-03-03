using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class FieldDisplayAttribute : Attribute
    {
        private bool _multiline;

        public FieldDisplayAttribute()
        {
            
        }

        public bool Multiline
        {
            get { return _multiline; }
            set { _multiline = value; }
        }
    }
}
