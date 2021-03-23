using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    public sealed class DocAttribute : Attribute
    {
        public string Doc { get; private set; }

        public DocAttribute(string doc)
        {
            this.Doc = doc;
        }
    }
}
