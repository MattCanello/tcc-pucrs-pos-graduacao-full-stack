using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces
{
    public interface IElasticSearchExceptionFactory
    {
        IndexException CreateExceptionFromResponse(IndexResponse indexResponse);
    }
}