using System;
using System.Collections.Generic;
using System.Text;

namespace Commander.Converters
{
    internal sealed class SByteConverter : CommandArgumentConverter<sbyte>
    {
        public override bool TryConvert(string value, out sbyte result)
        {
            return sbyte.TryParse(value, out result);
        }
    }
}
