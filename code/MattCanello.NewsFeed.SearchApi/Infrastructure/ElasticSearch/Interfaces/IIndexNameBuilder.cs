using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces
{
    public interface IIndexNameBuilder
    {
        IndexName? Build();
        IIndexNameBuilder WithFeedId(string feedId);
    }
}