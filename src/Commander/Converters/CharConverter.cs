using System;
using System.Collections.Generic;
using System.Text;

namespace Commander.Converters
{
    internal sealed class CharConverter : CommandArgumentTypeConverter<char>
    {
        public override bool TryConvert(string value, out char result)
        {
            return char.TryParse(value, out result);
        }
    }
}
