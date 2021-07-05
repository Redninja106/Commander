﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Commander.Converters
{
    internal sealed class FloatConverter : CommandArgumentTypeConverter<float>
    {
        public override bool TryConvert(string value, out float result)
        {
            return float.TryParse(value, out result);
        }
    }
}
