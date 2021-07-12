using System.Collections.Generic;
using System.IO;

namespace Commander
{
    /// <summary>
    /// Parses commands into CommandInvocations.
    /// </summary>
    internal class CommandParser
    {
        /// <summary>
        /// Gets a <see cref="CommandInvocation"/> from a string.
        /// </summary>
        /// <param name="commandString">The string to create the <see cref="CommandInvocation"/> from.</param>
        /// <returns>The new <see cref="CommandInvocation"/>.</returns>
        public CommandInvocation GetInvocation(string commandString)
        {
            using var sr = new StringReader(commandString);

            var fullName = sr.ReadWord();

            var parameters = new List<string>();

            while (sr.Peek() != -1f)
            {
                parameters.Add(sr.ReadWord());
            }

            var nameIndex = fullName.LastIndexOf(':') + 1;

            string service, name;
            if (nameIndex == -1)
            {
                service = null;
                name = fullName;
            }
            else
            {
                service = fullName[..nameIndex];
                name = fullName[nameIndex..];
            }

            return new CommandInvocation(service, name, parameters.ToArray());
        }
    }
}
