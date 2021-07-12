using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    /// <summary>
    /// Base class for a type converter. It is recommended to inherit from <see cref="CommandArgumentConverter{T}"/> instead.
    /// </summary>
    public abstract class CommandArgumentConverter
    {
        /// <summary>
        /// Gets the type which this converter converts into. The return value of this method should not change.
        /// </summary>
        /// <returns>The type which this converter converts into.</returns>
        public abstract Type GetTargetType();
        
        /// <summary>
        /// Attempts to convert a string value to an object of this converter's type. This may be called multiple times per command invocation.
        /// </summary>
        /// <param name="value">The string value to convert from.</param>
        /// <param name="result">The resulting object. If the conversion fails, this is null.</param>
        /// <returns>A boolean indicating if conversion was successful.</returns>
        public abstract bool TryConvertToObject(string value, out object result);
    }

    /// <summary>
    /// Base class for a type converter. Generic Wrapper for <see cref="CommandArgumentConverter"/>.
    /// </summary>
    /// <typeparam name="T">The type which this converter converts to.</typeparam>
    public abstract class CommandArgumentConverter<T> : CommandArgumentConverter
    {
        /// <summary>
        /// Attempts to convert a string value to an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The string value to convert from.</param>
        /// <param name="result">The resulting object for the conversion. If the conversion fails, this is null.</param>
        /// <returns>A boolean indicating if conversion was successful.</returns>
        public abstract bool TryConvert(string value, out T result);

        /// <inheritdoc/>
        public override Type GetTargetType()
        {
            return typeof(T);
        }

        /// <inheritdoc/>
        public override bool TryConvertToObject(string value, out object result)
        {
            result = TryConvert(value, out T res) ? (object)res : null;
            return result != null;
        }
    }
}
