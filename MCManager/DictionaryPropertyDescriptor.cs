using MCManager.Plugin_API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MCManager
{
    class DictionaryPropertyDescriptor<T,U> : PropertyDescriptor
    {
        Dictionary<T,U> _dictionary;
        T _key;

        internal DictionaryPropertyDescriptor(Dictionary<T, U> d, T key)
            : base(key.ToString(), null)
        {
            _dictionary = d;
            _key = key;
        }

        public override Type PropertyType
        {
            get { return _dictionary[_key].GetType(); }
        }

        public override void SetValue(object component, object value)
        {
            _dictionary[_key] = (U)value;
        }

        public override object GetValue(object component)
        {
            return _dictionary[_key];
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
