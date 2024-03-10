using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using System.Text.RegularExpressions;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Processors
{
    public sealed class QueryStringProcessor : IQueryStringProcessor
    {
        private const RegexOptions DefaultRegexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline;
        private static readonly Regex UnsafeCharsPattern = new Regex("\\*|\\?|\\:|\\'|\\\"|\\+|\\-|\\(|\\)|\\\\|\\/|\\<|\\>|\\[|\\]|\\{|\\}", DefaultRegexOptions);
        private static readonly Regex OrOperatorPattern = new Regex("(\\b|^)or(\\b|$)", DefaultRegexOptions);
        private static readonly Regex NotOperatorPattern = new Regex("(\\b|^)not(\\b|$)", DefaultRegexOptions);
        private static readonly Regex AndOperatorPattern = new Regex("(\\b|^)and(\\b|$)", DefaultRegexOptions);
        private static readonly Regex WhiteSpacePattern = new Regex("\\s+", DefaultRegexOptions);
        private static readonly Regex MultilinePattern = new Regex("\\r\\n|\\r|\\n", DefaultRegexOptions | RegexOptions.Multiline);

        public string Process(string? query)
        {
            if (string.IsNullOrEmpty(query))
                return "*";

            query = EnsureSingleLine(query);

            query = StripUnsafeChars(query);

            query = query.ToLowerInvariant();

            query = RemoveOperatorWords(query);

            query = RemoveMultipleWhiteSpaces(query);

            query = SurroundEachWordWithWildcard(query);

            return query;
        }

        public static string EnsureSingleLine(string query)
        {
            if (string.IsNullOrEmpty(query))
                return string.Empty;

            query = MultilinePattern.Replace(query, " ");

            return query;
        }

        public static string StripUnsafeChars(string query)
        {
            if (string.IsNullOrEmpty(query))
                return string.Empty;

            query = UnsafeCharsPattern.Replace(query, string.Empty);

            return query;
        }

        public static string RemoveOperatorWords(string query)
        {
            if (string.IsNullOrEmpty(query))
                return string.Empty;

            query = OrOperatorPattern.Replace(query, string.Empty);
            query = NotOperatorPattern.Replace(query, string.Empty);
            query = AndOperatorPattern.Replace(query, string.Empty);

            return query.Trim();
        }

        public static string RemoveMultipleWhiteSpaces(string query)
        {
            if (string.IsNullOrEmpty(query))
                return string.Empty;

            query = WhiteSpacePattern.Replace(query, " ");

            return query;
        }

        public static string SurroundEachWordWithWildcard(string query)
            => $"*{query.Replace(" ", "* *")}*";
    }
}
