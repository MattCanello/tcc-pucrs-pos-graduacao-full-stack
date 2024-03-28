using Nest;

namespace MattCanello.NewsFeed.Cross.ElasticSearch.Extensions
{
    public static class ResponseBaseExtensions
    {
        public static bool IsNotFound(this ResponseBase response)
            => response.ApiCall?.HttpStatusCode == 404;
    }
}
