using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    public abstract class CommandArgumentTypeConverter
    {
        public abstract Type GetTargetType();
        public abstract bool TryConvertToObject(string value, out object result);
    }

    public abstract class CommandArgumentTypeConverter<T> : CommandArgumentTypeConverter
    {
        public abstract bool TryConvert(string value, out T result);

        public override Type GetTargetType()
        {
            return typeof(T);
        }

        public override bool TryConvertToObject(string value, out object result)
        {
            result = TryConvert(value, out T res) ? (object)res : null;
            return result != null;
        }
    }
}
