using System;
using System.Collections.Generic;
using System.Text;

namespace Commander.Converters
{
    internal sealed class ByteConverter : CommandArgumentTypeConverter<byte>
    {
        public override bool TryConvert(string value, out byte result)
        {
            return byte.TryParse(value, out result);
        }
    }
}
