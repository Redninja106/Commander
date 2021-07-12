using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Commander
{
    /// <summary>
    /// An enviroment for executing commands.
    /// </summary>
    public sealed class CommandContext
    {
        // error message for an unrecognized command.
        private const string ERROR_COMMAND_NOT_RECOGNIZED = "Unrecognized Command:";
        
        // error for an ambiguous command invocation.
        private const string ERROR_AMBIGUOUS_CALL = "Ambiguous Call! Multiple commands referenced:";

        // List of all commands available to this context.
        private List<Command> commands = new List<Command>();
        
        // CommandParser for this context.
        private CommandParser commandParser = new CommandParser();

        // the desciption for the last error.
        private string lastError = null;

        /// <summary>
        /// Creates a new instance of the <see cref="CommandContext"/> class.
        /// </summary>
        public CommandContext()
        {
            // search all non-system assemblies for commands and register them.
            var assemblies = CommandRegistry.GetAllNonSystemAssemblies();

            foreach (var asm in assemblies)
            {
                commands.AddRange(CommandRegistry.GetCommands(asm));
            }
        }
        
        /// <summary>
        /// Registers a new command with this context, ignoring any instances of <see cref="CommandContextAttribute"/>.
        /// </summary>
        /// <param name="method">A <see cref="MethodInfo"/> representing a command. </param>
        public void RegisterCommand(MethodInfo method)
        {
            commands.Add(new Command(method));
        }
        
        /// <summary>
        /// Invokes a command from the specified string.
        /// </summary>
        /// <param name="commandString">The command syntax to parse and invoke.</param>
        public void SubmitCommand(string commandString)
        {
            if (! TrySubmitCommand(commandString))
            {
                throw new Exception(GetLastErrorDescription());
            }
        }

        /// <summary>
        /// Attempts to invoke a command from the specified string.
        /// </summary>
        /// <param name="commandString">The command syntax to parse and invoke.</param>
        /// <returns>A boolean value indicating whether the invocation succeeded or not. If this value is false, <see cref="GetLastErrorDescription"/> will return a value explaining the problem.</returns>
        public bool TrySubmitCommand(string commandString)
        {
            var invocation = commandParser.GetInvocation(commandString);

            var matchedCommands = commands.Where(cmd => cmd.IsInvocationValid(invocation));

            if (! matchedCommands.Any())
            {
                // command not recognized
                lastError = $"{ERROR_COMMAND_NOT_RECOGNIZED} '{invocation.Service}{invocation.Name}[parameters:{invocation.ParameterCount}]'";
                return false;
            }

            if (matchedCommands.Count() > 1)
            {
                // ambiguous call
                lastError = ERROR_AMBIGUOUS_CALL + Environment.NewLine;

                foreach (var cmd in matchedCommands)
                {
                    lastError += $"\t'{cmd}'{Environment.NewLine}";
                }

                return false;
            }

            // count is definitely 1 here

            var command = matchedCommands.First();

            if (! command.TryInvoke(invocation, out _, out string error))
            {
                lastError = error;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the description of the last error that occurred within this context.
        /// </summary>
        /// <returns>The error description.</returns>
        public string GetLastErrorDescription()
        {
            return lastError;
        }

        /// <summary>
        /// Reports an error to this context.
        /// </summary>
        /// <param name="error">A string describing the error. The user can get this string using <see cref="GetLastErrorDescription"/>.</param>
        internal void ReportError(string error)
        {
            lastError = error;
        }
    }
}
