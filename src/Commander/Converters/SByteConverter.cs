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
