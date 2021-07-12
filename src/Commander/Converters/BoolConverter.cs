namespace Commander.Converters
{
    internal sealed class BoolConverter : CommandArgumentConverter<bool>
    {
        public override bool TryConvert(string value, out bool result)
        {
            return bool.TryParse(value, out result);
        }
    }
}
