using Elasticsearch.Net;

namespace MattCanello.NewsFeed.Cross.ElasticSearch.Extensions
{
    public static class ServerErrorExtensions
    {
        public static bool IsIndexNotFound(this ServerError? serverError)
            => serverError?.Status == 404;
    }
}
