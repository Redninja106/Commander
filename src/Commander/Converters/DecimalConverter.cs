namespace Commander.Converters
{
    internal sealed class DecimalConverter : CommandArgumentConverter<decimal>
    {
        public override bool TryConvert(string value, out decimal result)
        {
            return decimal.TryParse(value, out result);
        }
    }
}
