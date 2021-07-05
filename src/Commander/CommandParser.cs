using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Commander
{
    /// <summary>
    /// Parses commands into CommandInvocations.
    /// </summary>
    internal class CommandParser
    {
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
