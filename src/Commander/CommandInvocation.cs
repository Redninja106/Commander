using System;
using System.Linq;

namespace Commander
{
    /// <summary>
    /// Represents a parsed command call. Is not garenteed to call a valid command in any <see cref="CommandContext"/>.
    /// </summary>
    internal sealed class CommandInvocation : IEquatable<CommandInvocation>
    {
        /// <summary>
        /// The service of the command being invoked.
        /// </summary>
        public string Service { get; private set; }

        /// <summary>
        /// The name of the command being invoked.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The number of parameters on the command being invoked.
        /// </summary>
        public int ParameterCount => Parameters.Length;

        /// <summary>
        /// The provided parameters of the command being invoked.
        /// </summary>
        public string[] Parameters { get; private set; }

        /// <summary>
        /// Creates a new CommandInvocation.
        /// </summary>
        /// <param name="service">The service of the command being invoked.</param>
        /// <param name="name">The name of the command being invoked.</param>
        /// <param name="parameters">The provided parameters of the command being invoked.</param>
        public CommandInvocation(string service, string name, string[] parameters)
        {
            this.Service = service.ToLower();
            this.Name = name.ToLower();
            this.Parameters = parameters;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (this.GetType() == obj.GetType())
            {
                return this.Equals((CommandInvocation)obj);
            }

            return base.Equals(obj);
        }

        /// <inheritdoc cref="Equals(object)"/>
        public bool Equals(CommandInvocation obj)
        {
            return this.Name == obj.Name &&
                this.Service == obj.Service &&
                this.Parameters.SequenceEqual(obj.Parameters);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Service, this.Name, this.ParameterCount, this.Parameters);
        }
    }
}
