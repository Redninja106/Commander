namespace Commander.Converters
{
    internal sealed class CharConverter : CommandArgumentConverter<char>
    {
        public override bool TryConvert(string value, out char result)
        {
            return char.TryParse(value, out result);
        }
    }
}
