namespace Commander.Converters
{
    internal sealed class ByteConverter : CommandArgumentConverter<byte>
    {
        public override bool TryConvert(string value, out byte result)
        {
            return byte.TryParse(value, out result);
        }
    }
}
