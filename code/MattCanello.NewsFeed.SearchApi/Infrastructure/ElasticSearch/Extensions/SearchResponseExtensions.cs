using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Extensions
{
    static class SearchResponseExtensions
    {
        public static bool IsIndexNotFound<TDocument>(this ISearchResponse<TDocument> response)
            where TDocument : class
            => response.ServerError?.IsIndexNotFound() ?? false;
    }
}
