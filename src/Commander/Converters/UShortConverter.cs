namespace Commander.Converters
{
    internal sealed class UShortConverter : CommandArgumentConverter<ushort>
    {
        public override bool TryConvert(string value, out ushort result)
        {
            return ushort.TryParse(value, out result);
        }
    }
}
