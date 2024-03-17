using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Builders
{
    public sealed class IndexNameBuilder : IIndexNameBuilder
    {
        private string? _feedId;
        private bool _asAllEntriesIndices;

        public IIndexNameBuilder WithFeedId(string feedId)
        {
            _feedId = feedId;

            return this;
        }

        public IIndexNameBuilder AllEntriesIndices()
        {
            _asAllEntriesIndices = true;

            return this;
        }

        public IndexName? Build()
        {
            if (_asAllEntriesIndices)
                return "entries-*";

            if (string.IsNullOrEmpty(_feedId))
                return null;

            return $"entries-{_feedId}";
        }
    }
}
