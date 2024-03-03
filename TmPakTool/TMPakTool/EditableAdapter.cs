using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace paktool
{
    abstract class EditableAdapterBase
    {
        private object _target;

        public EditableAdapterBase(object target)
        {
            _target = target;
        }

        public object Target
        {
            get { return _target; }
        }

        protected class PassthroughTypeDescriptionProvider : TypeDescriptionProvider
        {
            public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
            {
                if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(EditableAdapter<>))
                    return new PassthroughTypeDescriptor(objectType.GetGenericArguments()[0]);

                return base.GetTypeDescriptor(objectType, instance);
            }
        }

        protected class PassthroughTypeDescriptor : CustomTypeDescriptor
        {
            private Type _innerType;

            public PassthroughTypeDescriptor(Type innerType)
            {
                _innerType = innerType;
            }

            public override PropertyDescriptorCollection GetProperties()
            {
                PropertyDescriptorCollection properties = new PropertyDescriptorCollection(null);
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(_innerType))
                {
                    properties.Add(new PassthroughPropertyDescriptor(property));
                }
                return properties;
            }

            public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                return GetProperties();
            }
        }

        protected class PassthroughPropertyDescriptor : PropertyDescriptor
        {
            private PropertyDescriptor _innerProp;

            public PassthroughPropertyDescriptor(PropertyDescriptor innerProp)
                : base(innerProp)
            {
                _innerProp = innerProp;
            }

            public override object GetValue(object component)
            {
                return _innerProp.GetValue(((EditableAdapterBase)component).Target);
            }

            public override void SetValue(object component, object value)
            {
                _innerProp.SetValue(((EditableAdapterBase)component).Target, value);
            }

            public override bool CanResetValue(object component)
            {
                return _innerProp.CanResetValue(component);
            }

            public override Type ComponentType
            {
                get { return _innerProp.ComponentType; }
            }

            public override bool IsReadOnly
            {
                get { return _innerProp.IsReadOnly; }
            }

            public override Type PropertyType
            {
                get { return _innerProp.PropertyType; }
            }

            public override void ResetValue(object component)
            {
                _innerProp.ResetValue(component);
            }

            public override bool ShouldSerializeValue(object component)
            {
                return _innerProp.ShouldSerializeValue(component);
            }
        }

        protected class ObjectState
        {
            private object _target;
            private Dictionary<PropertyDescriptor, object> _origPropValues;

            public ObjectState(object target)
            {
                _target = target;
                _origPropValues = new Dictionary<PropertyDescriptor, object>();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(target))
                {
                    _origPropValues.Add(property, property.GetValue(target));
                }
            }

            public void Apply()
            {
                if (_origPropValues == null)
                    return;

                foreach (KeyValuePair<PropertyDescriptor, object> pair in _origPropValues)
                {
                    pair.Key.SetValue(_target, pair.Value);
                }
                _origPropValues = null;
            }
        }
    }

    [TypeDescriptionProvider(typeof(PassthroughTypeDescriptionProvider))]
    class EditableAdapter<T> : EditableAdapterBase, IEditableObject, INotifyPropertyChanged
    {
        private ObjectState _origState;

        public EditableAdapter(object target)
            : base(target)
        {
            if (Target is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)Target).PropertyChanged +=
                    delegate(object sender, PropertyChangedEventArgs args)
                    {
                        if (PropertyChanged != null)
                            PropertyChanged(this, args);
                    };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void BeginEdit()
        {
            _origState = new ObjectState(Target);
        }

        public void CancelEdit()
        {
            if (_origState != null)
            {
                _origState.Apply();
                _origState = null;
            }
        }

        public void EndEdit()
        {
            _origState = null;
        }
    }
}
