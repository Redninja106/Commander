using System;
using System.Collections.Generic;
using System.Text;

namespace Commander.Converters
{
    internal sealed class UShortConverter : CommandArgumentTypeConverter<ushort>
    {
        public override bool TryConvert(string value, out ushort result)
        {
            return ushort.TryParse(value, out result);
        }
    }
}
