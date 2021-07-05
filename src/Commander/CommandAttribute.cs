using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    /// <summary>
    /// Marks a static method as a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CommandAttribute : Attribute
    {
        /// <summary>
        /// The full name of the service which this command should be placed within.
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Marks a method as a command. The target method must be static or invocation of this command will fail. 
        /// </summary>
        /// <param name="serviceName">The name of the service which this command should be located in. Passing null or leaving the default value places the command in a namespace based off the CommandContext's initial options.</param>
        public CommandAttribute(string serviceName = null)
        {
            this.ServiceName = serviceName;
        }
    }
}
