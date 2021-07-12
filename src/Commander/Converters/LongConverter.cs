namespace Commander.Converters
{
    internal sealed class LongConverter : CommandArgumentConverter<long>
    {
        public override bool TryConvert(string value, out long result)
        {
            return long.TryParse(value, out result);
        }
    }
}
