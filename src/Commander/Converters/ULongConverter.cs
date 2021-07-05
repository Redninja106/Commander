using System;
using System.Collections.Generic;
using System.Text;

namespace Commander.Converters
{
    internal sealed class ULongConverter : CommandArgumentTypeConverter<ulong>
    {
        public override bool TryConvert(string value, out ulong result)
        {
            return ulong.TryParse(value, out result);
        }
    }
}
