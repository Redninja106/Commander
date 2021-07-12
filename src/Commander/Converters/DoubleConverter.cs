namespace Commander.Converters
{
    internal sealed class DoubleConverter : CommandArgumentConverter<double>
    {
        public override bool TryConvert(string value, out double result)
        {
            return double.TryParse(value, out result);
        }
    }
}
