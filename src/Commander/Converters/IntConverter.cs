namespace Commander.Converters
{
    internal sealed class IntConverter : CommandArgumentConverter<int>
    {
        public override bool TryConvert(string value, out int result)
        {
            return int.TryParse(value, out result);
        }
    }
}
