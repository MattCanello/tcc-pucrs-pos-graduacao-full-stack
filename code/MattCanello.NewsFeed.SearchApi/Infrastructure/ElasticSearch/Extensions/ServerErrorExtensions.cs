using Elasticsearch.Net;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Extensions
{
    static class ServerErrorExtensions
    {
        public static bool IsIndexNotFound(this ServerError? serverError) 
            => serverError?.Status == 404;
    }
}
