namespace Commander.Converters
{
    internal sealed class FloatConverter : CommandArgumentConverter<float>
    {
        public override bool TryConvert(string value, out float result)
        {
            return float.TryParse(value, out result);
        }
    }
}
