using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Commander
{
    /// <summary>
    /// Conatins various method for finding elements within assemblies and, more specifically, finding commands for command contexts.
    /// </summary>
    internal static class CommandRegistry
    {
        public static Command[] AllCommands { get; private set; }

        public static Dictionary<Type, CommandArgumentTypeConverter> ArgumentConverters { get; private set; }

        static CommandRegistry()
        {
            List<Command> cmds = new List<Command>();
            var converters = new Dictionary<Type, CommandArgumentTypeConverter>();

            foreach (var asm in GetAllNonSystemAssemblies())
            {
                cmds.AddRange(GetCommands(asm));

                foreach (var converter in GetCommandArgumentConverters(asm))
                {
                    converters.Add(converter.GetTargetType(), converter);
                }
            }

            AllCommands = cmds.ToArray();
            ArgumentConverters = converters;
        }

        public static Command[] GetCommands(Assembly assembly)
        {
            var commands = new List<Command>();

            foreach (var type in assembly.GetTypes())
            {
                commands.AddRange(type.GetMethods().Where(method => method.GetCustomAttribute<CommandAttribute>() != null).Select(m => new Command(m)));
            }

            return commands.ToArray();
        }

        public static CommandArgumentTypeConverter[] GetCommandArgumentConverters(Assembly assembly)
        {
            var converters = new List<CommandArgumentTypeConverter>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(CommandArgumentTypeConverter)))
                {
                    var ctor = type.GetConstructor(Array.Empty<Type>());
                    if (ctor != null)
                    {
                        converters.Add((CommandArgumentTypeConverter)ctor.Invoke(Array.Empty<object>()));
                    }
                }
            }

            return converters.ToArray();
        }

        public static Assembly[] GetAllNonSystemAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.GetName().Name.StartsWith("System")).ToArray();
        }
    }
}
