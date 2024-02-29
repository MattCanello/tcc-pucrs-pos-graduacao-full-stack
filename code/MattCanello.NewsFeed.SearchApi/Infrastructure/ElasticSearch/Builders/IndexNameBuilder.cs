using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Builders
{
    public sealed class IndexNameBuilder : IIndexNameBuilder
    {
        private string? _feedId;

        public IIndexNameBuilder WithFeedId(string feedId)
        {
            _feedId = feedId;

            return this;
        }

        public IndexName? Build()
        {
            if (string.IsNullOrEmpty(_feedId))
                return null;

            return $"entries-{_feedId}";
        }
    }
}
