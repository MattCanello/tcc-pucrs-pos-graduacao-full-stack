using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Factories
{
    public sealed class ElasticSearchExceptionFactory : IElasticSearchExceptionFactory
    {
        public IndexException CreateExceptionFromResponse(IndexResponse indexResponse)
        {
            var exception = new IndexException(
                indexResponse.ServerError?.Error?.Reason!,
                indexResponse.OriginalException);

            if (indexResponse.ServerError != null)
                exception.Data["elasticsearch-error"] = indexResponse.ServerError;

            return exception;
        }
    }
}
