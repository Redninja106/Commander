namespace Commander.Converters
{
    internal sealed class UIntConverter : CommandArgumentConverter<uint>
    {
        public override bool TryConvert(string value, out uint result)
        {
            return uint.TryParse(value, out result);
        }
    }
}
