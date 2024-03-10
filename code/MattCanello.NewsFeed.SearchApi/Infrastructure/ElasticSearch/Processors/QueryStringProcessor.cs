using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Processors
{
    public sealed class QueryStringProcessor : IQueryStringProcessor
    {
        public string Process(string? query)
        {
            if (string.IsNullOrEmpty(query))
                return "*";

            query = query
                .Replace("*", "")
                .Replace("?", "")
                .Replace(":", "")
                .Replace("'", "")
                .Replace("+", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("\"", "")
                .ToLowerInvariant()
                .Replace(" or ", " ")
                .Replace(" not ", " ")
                .Replace(" and ", " ")
                .Replace(" ", "* *");

            return $"*{query}*";
        }
    }
}
