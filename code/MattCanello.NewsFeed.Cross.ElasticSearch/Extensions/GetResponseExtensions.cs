using Nest;

namespace MattCanello.NewsFeed.Cross.ElasticSearch.Extensions
{
    public static class GetResponseExtensions
    {
        public static bool IsDocumentNotFound<TDocument>(this GetResponse<TDocument> response)
            where TDocument : class
        {
            var isEntryNotFound = response.IsNotFound()
                || (response.ServerError is null && !response.Found)
                || (response.Found && response.Source is null);

            return isEntryNotFound;
        }
    }
}
