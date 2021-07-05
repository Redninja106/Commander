using System;
using System.Collections.Generic;
using System.Text;

namespace Commander.Converters
{
    internal sealed class LongConverter : CommandArgumentTypeConverter<long>
    {
        public override bool TryConvert(string value, out long result)
        {
            return long.TryParse(value, out result);
        }
    }
}
