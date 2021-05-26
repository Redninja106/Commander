using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    /// <summary>
    /// Command documentation attribute for optional command arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class OpDocArgAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of the <see cref="OpDocArgAttribute"/> class.
        /// </summary>
        /// <param name="type">The type of value of the argument.</param>
        /// <param name="name">The name of the of the argument.</param>
        /// <param name="defaultValue">The default value of the arugment.</param>
        /// <param name="doc">The description of the argument.</param>
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
