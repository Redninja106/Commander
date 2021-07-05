using System;
using System.Collections.Generic;
using System.Text;

namespace Commander.Converters
{
    internal sealed class BoolConverter : CommandArgumentTypeConverter<bool>
    {
        public override bool TryConvert(string value, out bool result)
        {
            return bool.TryParse(value, out result);
        }
    }
}
