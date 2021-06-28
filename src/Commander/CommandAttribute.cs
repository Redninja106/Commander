using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    public sealed class CommandAttribute : Attribute
    {
        public CommandAttribute(string serviceName = "")
        {
            this.serviceName = serviceName;
        }

        internal string serviceName;
    }
}
