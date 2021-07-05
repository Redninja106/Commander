using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    /// <summary>
    /// Indicates that a command should be automatically added to a command context of a certain name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class CommandContextAttribute : Attribute
    {
        public string ContextName { get; }

        /// <summary>
        /// Indicates that a command should be automatically added to a command context of the provided name. Passing null indicates that this command should not be added to the global context.
        /// </summary>
        /// <param name="contextName"></param>
        public CommandContextAttribute(string contextName)
        {
            this.ContextName = contextName;
        }
    }
}
