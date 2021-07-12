using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Commander
{
    /// <summary>
    /// Conatins various method for finding elements within assemblies and, more specifically, finding commands for command contexts.
    /// </summary>
    internal static class CommandRegistry
    {
        /// <summary>
        /// Every command registered to every context. Used to prevent duplicate commands.
        /// </summary>
        public static Command[] AllCommands { get; private set; }

        /// <summary>
        /// Every defined ArgumentConverter in the scanned namespaces.
        /// </summary>
        public static Dictionary<Type, CommandArgumentConverter> ArgumentConverters { get; private set; }

        // scans all loaded non-system assemblies for type converters and commands.
        static CommandRegistry()
        {
            List<Command> cmds = new List<Command>();
            var converters = new Dictionary<Type, CommandArgumentConverter>();

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

        /// <summary>
        /// Finds and returns all of the commands within an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search for commands.</param>
        /// <returns>An array of the commands defined in the assembly.</returns>
        public static Command[] GetCommands(Assembly assembly)
        {
            var commands = new List<Command>();

            foreach (var type in assembly.GetTypes())
            {
                commands.AddRange(type.GetMethods().Where(method => method.GetCustomAttribute<CommandAttribute>() != null).Select(m => new Command(m)));
            }

            return commands.ToArray();
        }

        /// <summary>
        /// Finds and the types that inherit <see cref="CommandArgumentConverter"/> within an assembly and returns an array with one instance of each.
        /// </summary>
        /// <param name="assembly">The assembly to search for converters.</param>
        /// <returns>An array of the converters defined in the assembly.</returns>
        public static CommandArgumentConverter[] GetCommandArgumentConverters(Assembly assembly)
        {
            var converters = new List<CommandArgumentConverter>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(CommandArgumentConverter)))
                {
                    var ctor = type.GetConstructor(Array.Empty<Type>());
                    if (ctor != null)
                    {
                        converters.Add((CommandArgumentConverter)ctor.Invoke(Array.Empty<object>()));
                    }
                }
            }

            return converters.ToArray();
        }

        /// <summary>
        /// Gets all loaded assemblies not in the "System" namespace.
        /// </summary>
        public static Assembly[] GetAllNonSystemAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.GetName().Name.StartsWith("System")).ToArray();
        }
    }
}
