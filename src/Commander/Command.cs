using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Commander
{
    internal sealed class Command
    {
        private delegate bool TryParseFunc<T>(string value, out T result);

        static Type[] ProcessParameters(ParameterInfo[] parameters, out int lastResultIndex)
        {
            if(parameters.Length == 0)
            {
                lastResultIndex = -1;
                return new Type[0];
            }

            var result = new List<Type>(parameters.Length - 1);
            lastResultIndex = -1;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].GetCustomAttribute<LastResultAttribute>() != null)
                {
                    lastResultIndex = i;
                }
                else
                {
                    result.Add(parameters[i].ParameterType);
                } 
            }

            return result.ToArray();
        }

        public Command(MethodInfo info) : this(info.ReturnType, info.GetCustomAttribute<CommandAttribute>().serviceName.ToLower(), info.Name.ToLower(), ProcessParameters(info.GetParameters(), out int lastResultIndex), lastResultIndex, info)
        {
        }

        public Command(MethodInfo info, string serviceName) : this(info.ReturnType, serviceName.ToLower(), info.Name.ToLower(), ProcessParameters(info.GetParameters(), out int lastResultIndex), lastResultIndex, info)
        {
        }

        public Command(Type returnType, string nameSpace, string name, Type[] parameters, int lastResultIndex, MethodInfo methodInfo)
        {
            ReturnType = returnType;
            Signature = new CommandSignature(nameSpace, name, new List<Type>(parameters));
            LastResultIndex = lastResultIndex;
            MethodInfo = methodInfo;
        }

        /// <summary>
        /// The type of object returned by the command.
        /// </summary>
        public Type ReturnType { get; private set; }

        public CommandSignature Signature { get; private set; }

        /// <summary>
        /// The index in the argument array where the return value of the last command should be inserted.
        /// </summary>
        public int LastResultIndex { get; private set; }

        /// <summary>
        /// A <see cref="System.Reflection.MethodInfo"/> for the method the command maps to.
        /// </summary>
        public MethodInfo MethodInfo { get; private set; }

        /// <summary>
        /// Invokes the command with the specified last result and arguments.
        /// </summary>
        /// <param name="lastResult"></param>
        /// <param name="args"></param>
        /// <param name="commandResult">The object returned by the command.</param>
        public bool Invoke(object lastResult, object[] args, out object commandResult)
        {
            commandResult = null;

            var argsWithLastResult = new List<object>(args); 

            if (LastResultIndex > -1)
            {

                if (lastResult != null && !MethodInfo.GetParameters()[LastResultIndex].ParameterType.IsAssignableFrom(lastResult.GetType()))
                {
                    lastResult = null;
                }

                argsWithLastResult.Insert(LastResultIndex, lastResult);
            }

            var parameters = MethodInfo.GetParameters();
            if(argsWithLastResult.Count < parameters.Length)
            {
                for (int i = argsWithLastResult.Count; i < parameters.Length; i++)
                {
                    argsWithLastResult.Add(parameters[i].DefaultValue);
                }
            }

            int argsIndex;
            bool parseSuccess = true;

            void TryParse<T>(TryParseFunc<T> parse)
            {
                parseSuccess = parse(argsWithLastResult[argsIndex].ToString(), out T result);
                if (parseSuccess)
                    argsWithLastResult[argsIndex] = result;
            }

            for (argsIndex = 0; argsIndex < parameters.Length; argsIndex++)
            {
                if (parameters[argsIndex].ParameterType != typeof(string) && argsIndex != LastResultIndex)
                {
                    switch (parameters[argsIndex].ParameterType.Name)
                    {
                        case nameof(Boolean):
                            TryParse<bool>(bool.TryParse);
                            break;
                        case nameof(SByte):
                            TryParse<sbyte>(sbyte.TryParse);
                            break;
                        case nameof(Byte):
                            TryParse<byte>(byte.TryParse);
                            break;
                        case nameof(Int16):
                            TryParse<short>(short.TryParse);
                            break;
                        case nameof(UInt16):
                            TryParse<ushort>(ushort.TryParse);
                            break;
                        case nameof(Int32):
                            TryParse<int>(int.TryParse);
                            break;
                        case nameof(UInt32):
                            TryParse<uint>(uint.TryParse);
                            break;
                        case nameof(Int64):
                            TryParse<long>(long.TryParse);
                            break;
                        case nameof(UInt64):
                            TryParse<ulong>(ulong.TryParse);
                            break;
                        case nameof(Single):
                            TryParse<float>(float.TryParse);
                            break;
                        case nameof(Double):
                            TryParse<double>(double.TryParse);
                            break;
                        case nameof(Decimal):
                            TryParse<decimal>(decimal.TryParse);
                            break;
                        case nameof(Char):
                            TryParse<char>(char.TryParse);
                            break;
                        case nameof(String):
                            parseSuccess = true;
                            break;
                        default:
                            return Service.ReportError($"Command {this.MethodInfo.Name} has parameter {parameters[argsIndex].Name}, which is of an unsupported type.");       
                    }
                    
                    if(!parseSuccess)
                        return Service.ReportError("Invalid argument at index \"" + argsIndex + "\".");
                }
            }

            commandResult = MethodInfo.Invoke(null, argsWithLastResult.ToArray());

            return true;
        }

        public override string ToString()
        {
            return Signature.ToString();
        }
    }
}
