using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Extensions
{
    static class GetResponseExtensions
    {
        public static bool IsEntryNotFound<TDocument>(this GetResponse<TDocument> response)
            where TDocument : class
        {
            var isEntryNotFound = response.IsNotFound()
                || (response.ServerError is null && !response.Found)
                || (response.Found && response.Source is null);

            return isEntryNotFound;
        }
    }
}
