using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Commander
{
    public static class Service
    {
        internal static OutputStyle Style = OutputStyle.Default;

        public static IConsole Output { get; set; }

        /// <summary>
        /// Every command that is currently registered.
        /// </summary>
        internal static List<Command> RegisteredCommands = new List<Command>();
        
        /// <summary>
        /// Invocation builder to parse commands.
        /// </summary>
        internal static InvocationBuilder invocationBuilder = new InvocationBuilder();

        public static string[] GetRegisteredCommands()
        {
            string[] result = new string[RegisteredCommands.Count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = RegisteredCommands[i].ToString();
            }

            return result;
        }

        /// <summary>
        /// The return value of the most recently invoked command.
        /// </summary>
        internal static object lastResult;

        static Service()
        {
            SetOutputToSystemConsole();

            // by default, register this assembly and the Entry one.
            RegisterAssembly(Assembly.GetEntryAssembly());
            RegisterAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Searches an assembly for commands and registers those commands. By default, the assembly returned by <see cref="Assembly.GetEntryAssembly"/> is searched. Libraries should call this upon their initialization.
        /// </summary>
        /// <param name="assembly"></param>
        public static void RegisterAssembly(Assembly assembly)
        {
            // register every type in the assembly.
            foreach (var type in assembly.DefinedTypes)
            {
                RegisterType(type);
            }
        }

        public static void RegisterType<T>()
        {
            RegisterType(typeof(T));
        }

        public static void RegisterType(Type type)
        {
            // First, get every public static method on the type
            foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                // If the method has a command addribute, try to register it. This method will throw if the method is invalid.
                if (method.GetCustomAttribute<CommandAttribute>() != null)
                {
                    RegisterMethod(method);
                }
            }
        }

        public static void RegisterMethod(MethodInfo info)
        {
            var attr = info.GetCustomAttribute<CommandAttribute>();

            // make sure there is no command of this name registered.
            if (RegisteredCommands.Any(c => c.Signature.Name.ToLower() == info.Name.ToLower() && c.Signature.ServiceName.ToLower() == attr.serviceName.ToLower()))
            {
                throw new Exception($"Command {info.Name.ToLower()} is already present in the service '{attr.serviceName.ToLower()}'");
            }

            // if the service name explicity defined, use that. Otherwise use the type name.
            string serviceName = attr.serviceName != "" ? attr.serviceName : info.DeclaringType.Name;

            // command is valid and the type name is known. All good to register.
            RegisteredCommands.Add(new Command(info, serviceName));
        }

        public static void SubmitCommandString(string commandString)
        {
            var invocations = invocationBuilder.GetInvocations(commandString);

            foreach (var invocation in invocations)
            {
                try
                {
                    InvokeCommand(invocation);
                }
                catch (Exception ex)
                {
                    Output.WriteLine(ex, Style.Error);
                }
            }
        }

        /// <summary>
        /// Invokes a command using a command invocation.
        /// </summary>
        /// <param name="invocation">The command invocation to invoke the command with.</param>
        public static void InvokeCommand(CommandInvocation invocation)
        {
            Command command;
            if (invocation.Service == "")
            {
                // no service was specified by the caller, just going have to make do with the name.
                // So, get all of the commands that have the providied name.
                var candidates = RegisteredCommands.Where(c => c.Signature.Name == invocation.Name);

                // There are no cadidates
                if (candidates.Count() < 1)
                {
                    throw new Exception($"Unrecognized command name: {invocation.Name}");
                }

                // more then one command with the provided name. No way to tell which to call. Uh oh.
                if (candidates.Count() > 1)
                {
                    // format an exception string
                    string exStr = "Ambiguous reference to two or more commands:";

                    foreach (var candidate in candidates)
                    {
                        exStr += "\n" + candidate.ToString();
                    }

                    throw new Exception(exStr);
                }

                command = candidates.First();
            }
            else
            {
                command = RegisteredCommands.Find(c => c.Signature.ServiceName == invocation.Service?.ToLower() && c.Signature.Name == invocation.Name?.ToLower());

                if (command == null)
                {
                    throw new Exception($"There is no registered command of name '{invocation}'!");
                }
            }
            try
            {
                lastResult = command.Invoke(lastResult, invocation.Parameters.ToArray());
            }
            catch (Exception ex)
            {
                Output.WriteLine(ex.InnerException?.Message ?? ex.Message, Style.Error);
            }
        }
        
        public static void SetOutputToSystemConsole()
        {
            Output = SystemConsole.Instance;
        }

        public static OutputStyle GetStyle()
        {
            return Style;
        }

        public static void SetStyle(OutputStyle style)
        {
            Style = style;
        }
    }
}