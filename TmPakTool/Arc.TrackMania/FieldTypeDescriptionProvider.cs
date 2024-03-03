using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Arc.TrackMania
{
    class FieldTypeDescriptionProvider<T> : TypeDescriptionProvider
        where T : class
    {
        private Dictionary<Type, FieldTypeDescriptor> _cache = new Dictionary<Type, FieldTypeDescriptor>();

        public FieldTypeDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(T)))
        {

        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            FieldTypeDescriptor descriptor;
            if (!_cache.TryGetValue(objectType, out descriptor))
            {
                descriptor = new FieldTypeDescriptor(base.GetTypeDescriptor(objectType, null), objectType);
                _cache.Add(objectType, descriptor);
            }
            return descriptor;
        }
    }
}
