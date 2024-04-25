namespace MattCanello.NewsFeed.RssReader.Domain.Extensions
{
    public static class StringExtensionMethods
    {
        public static string? ToNullWhenEmpty(this string? str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            return str;
        }
    }
}
