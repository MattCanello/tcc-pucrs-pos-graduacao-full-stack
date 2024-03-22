using Nest;

namespace MattCanello.NewsFeed.Cross.ElasticSearch.Extensions
{
    public static class SearchResponseExtensions
    {
        public static bool IsIndexNotFound<TDocument>(this ISearchResponse<TDocument> response)
            where TDocument : class
            => response.ServerError?.IsIndexNotFound() ?? false;
    }
}
