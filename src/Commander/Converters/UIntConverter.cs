using System;
using System.Collections.Generic;
using System.Text;

namespace Commander.Converters
{
    internal sealed class UIntConverter : CommandArgumentTypeConverter<uint>
    {
        public override bool TryConvert(string value, out uint result)
        {
            return uint.TryParse(value, out result);
        }
    }
}
