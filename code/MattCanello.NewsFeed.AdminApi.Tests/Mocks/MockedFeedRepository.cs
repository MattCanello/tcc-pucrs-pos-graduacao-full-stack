using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    sealed class MockedFeedRepository : IFeedRepository
    {
        private readonly IDictionary<string, Feed> _data;

        public MockedFeedRepository(Feed feed)
            : this(new Dictionary<string, Feed>(capacity: 1) { { feed.FeedId, feed } }) { }

        public MockedFeedRepository(IDictionary<string, Feed>? data = null)
        {
            _data = data ?? new Dictionary<string, Feed>();
        }

        public Task<Feed> CreateAsync(Feed feed, CancellationToken cancellationToken = default)
        {
            _data[feed.FeedId] = feed;

            return Task.FromResult(feed);
        }

        public Task<Feed?> GetByIdAsync(string feedId, CancellationToken cancellationToken = default)
        {
            if (_data.TryGetValue(feedId, out Feed? feed))
                return Task.FromResult<Feed?>(feed);

            return Task.FromResult<Feed?>(null);
        }

        public Task<Feed> UpdateAsync(Feed feed, CancellationToken cancellationToken = default)
        {
            _data[feed.FeedId] = feed;

            return Task.FromResult(feed);
        }
    }
}
