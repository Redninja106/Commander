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
