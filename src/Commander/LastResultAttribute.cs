using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class LastResultAttribute : Attribute
    {
    }
}
