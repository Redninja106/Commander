using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Commander
{
    /// <summary>
    /// Represents a command and gets information for a command based off it's implementation <see cref="MethodInfo"/>. 
    /// </summary>
    internal sealed class Command
    {
        /// <summary>
        /// The service the command is located in. This can be multiple services nested (i.e. ServiceA:ServiceB:).
        /// </summary>
        public string Service { get; private set; }
        /// <summary>
        /// The name of the referenced command.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The number of parameters that this command accepts. Parameters can have the same name as long as they have different parameter counts.
        /// </summary>
        public int ParameterCount => ParameterTypes.Length;
        
        /// <summary>
        /// An array of types representing the parameters for this command.
        /// </summary>
        public Type[] ParameterTypes { get; private set; }

        /// <summary>
        /// Reflection reference to the method which implements this command.
        /// </summary>
        private readonly MethodInfo method;

        public Command(MethodInfo method)
        {
            this.method = method;
            this.Name = method.Name;
            this.ParameterTypes = method.GetParameters().Select(param => param.ParameterType).ToArray();

            var hardcodedServiceName = method.GetCustomAttribute<CommandAttribute>().ServiceName;

            if (hardcodedServiceName == null)
            {
                // generate service name, hardcoded wasn't specified.
                Service = method.DeclaringType.Name;
            }
            else
            {
                // provided hardcoded name will do
                Service = hardcodedServiceName;
            }

            // all service names should be postfixed with a ":"
            if (! Service.EndsWith(':'))
            {
                Service += ':';
            }
        }

        /// <summary>
        /// invokes the command using the provided invocation, converting the arguments on the invocation using .
        /// </summary>
        /// <param name="invocation">The CommandInvocation which contains the parameters for the command.</param>
        /// <returns>The object returned by the command, or null if it the call fails.</returns>
        public bool TryInvoke(CommandInvocation invocation, out object result, out string errorDesc)
        {
            
            object[] convertedParameters = new object[invocation.ParameterCount];

            for (int i = 0; i < invocation.ParameterCount; i++)
            {
                var parameterType = this.ParameterTypes[i];

                var converter = CommandRegistry.ArgumentConverters[parameterType];

                if (! converter.TryConvertToObject(invocation.Parameters[i], out convertedParameters[i]))
                {
                    errorDesc = $"Unable to convert value '{invocation.Parameters[i]}' to type '{parameterType}'";
                    result = null;
                    return false;
                }
            }

            try 
            {
                result = method.Invoke(null, convertedParameters);
                errorDesc = null;
                return true;
            }
            catch (Exception ex)
            {
                errorDesc = ex.Message;
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Checks if the provided <see cref="CommandInvocation"/> would result in a success invocation of this command, and targets a command in this command's service.
        /// </summary>
        /// <param name="invocation">The <see cref="CommandInvocation"/> to check.</param>
        public bool IsInvocationValid(CommandInvocation invocation)
        {
            // invalid if name doesn't match
            if (Name.ToLower() != invocation.Name.ToLower())
            {
                return false;
            }

            // invalid if parameter count is different.
            if (ParameterCount != invocation.ParameterCount)
            {
                return false;
            }

            // if the invocation's service is unspecified, name and parameter count already matches, so this command is a valid candidate for an implicit call.
            if (string.IsNullOrEmpty(invocation.Service))
            {
                return true;
            }
            
            // invalid if services aren't equal.
            if (this.Service.ToLower() != invocation.Service.ToLower())
            {
                return false;
            }

            // everything matches.
            return true;
        }

        public override string ToString()
        {
            return $"{this.Service}{this.Name}[parameters:{this.ParameterCount}]";
        }
    }
}
