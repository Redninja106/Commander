using System;
using System.Collections.Generic;
using System.Text;

namespace Commander.Converters
{
    internal sealed class ShortConverter : CommandArgumentConverter<short>
    {
        public override bool TryConvert(string value, out short result)
        {
            return short.TryParse(value, out result);
        }
    }
}
