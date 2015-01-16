namespace Infrastructure.Common
{
    public static class StringExtensions
    {
        public static string AsString<T>(this T? item) where T : struct
        {
            return item.HasValue ? item.Value.ToString() : string.Empty;
        }
    }
}
