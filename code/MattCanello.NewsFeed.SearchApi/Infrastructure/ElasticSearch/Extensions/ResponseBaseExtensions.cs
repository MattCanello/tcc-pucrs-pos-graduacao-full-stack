using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Extensions
{
    static class ResponseBaseExtensions
    {
        public static bool IsNotFound(this ResponseBase response)
            => response.ApiCall?.HttpStatusCode == 404;
    }
}
