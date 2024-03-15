using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces
{
    public interface IElasticModelFactory
    {
        Entry CreateElasticModel(IndexEntryCommand command);
    }
}