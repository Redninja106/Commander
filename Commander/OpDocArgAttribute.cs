using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class OpDocArgAttribute : Attribute
    {
        public OpDocArgAttribute(string type, string name, string defaultValue, string doc)
        {
            Doc = doc;
            Type = type;
            Name = name;
            DefaultValue = defaultValue;
        }

        public string Doc { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }
        public string DefaultValue { get; private set; }

    }
}
