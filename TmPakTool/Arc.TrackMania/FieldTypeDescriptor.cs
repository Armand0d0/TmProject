using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Arc.TrackMania
{
    class FieldTypeDescriptor : CustomTypeDescriptor
    {
        private List<FieldPropertyDescriptor> _fieldDescriptors = new List<FieldPropertyDescriptor>();

        public FieldTypeDescriptor(ICustomTypeDescriptor parent, Type type)
            : base(parent)
        {
            foreach(FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                _fieldDescriptors.Add(new FieldPropertyDescriptor(type, field));
            }
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);
            foreach(PropertyDescriptor prop in base.GetProperties(attributes))
            {
                result.Add(prop);
            }
            foreach (FieldPropertyDescriptor descriptor in _fieldDescriptors)
            {
                result.Add(descriptor);
            }
            return result;
        }
    }
}
