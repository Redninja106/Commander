using System;
using System.Collections.Generic;
using System.Text;

namespace Commander.Documentation
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class DocArgAttribute : Attribute
    {
        public DocArgAttribute(string type, string name, string doc)
        {
            Type = type;
            Name = name;
            Doc = doc;
        }

        public string Type { get; private set; }
        public string Name { get; private set; }
        public string Doc { get; private set; }
    }
}
