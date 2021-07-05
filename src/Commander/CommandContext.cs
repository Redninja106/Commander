using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Commander
{
    public sealed class CommandContext
    {
        private const string ERROR_COMMAND_NOT_RECOGNIZED = "Unrecognized Command:";
        private const string ERROR_AMBIGUOUS_CALL = "Ambiguous Call! Multiple commands referenced:";

        private List<Command> commands = new List<Command>();
        
        private CommandParser commandParser = new CommandParser();

        private string lastError = null;

        public CommandContext()
        {
            var assemblies = CommandRegistry.GetAllNonSystemAssemblies();

            foreach (var asm in assemblies)
            {
                commands.AddRange(CommandRegistry.GetCommands(asm));
            }
        }
        
        public void RegisterCommand(MethodInfo method)
        {
            commands.Add(new Command(method));
        }
        
        public void SubmitCommand(string commandString)
        {
            if (! TrySubmitCommand(commandString))
            {
                throw new Exception(GetLastErrorDescription());
            }
        }

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

        public string GetLastErrorDescription()
        {
            return lastError;
        }

        internal void ReportError(string error)
        {
            lastError = error;
        }
    }
}
