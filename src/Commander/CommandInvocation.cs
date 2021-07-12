namespace Commander
{
    /// <summary>
    /// Represents a parsed command call. Is not garenteed to call a valid command in any <see cref="CommandContext"/>.
    /// </summary>
    internal sealed class CommandInvocation
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
            this.Service = service;
            this.Name = name;
            this.Parameters = parameters;
        }
    }
}
