using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Arc.TrackMania
{
    class FieldPropertyDescriptor : PropertyDescriptor
    {
        private Type _componentType;
        private FieldInfo _field;

        public FieldPropertyDescriptor(Type componentType, FieldInfo field)
            : base(field.Name, null)
        {
            _componentType = componentType;
            _field = field;
        }

        public override AttributeCollection Attributes
        {
            get
            {
                return new AttributeCollection(_field.GetCustomAttributes(false).Cast<Attribute>().ToArray());
            }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return _componentType; }
        }

        public override object GetValue(object component)
        {
            return _field.GetValue(component);
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return _field.FieldType; }
        }

        public override void ResetValue(object component)
        {
            
        }

        public override void SetValue(object component, object value)
        {
            _field.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
