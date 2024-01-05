using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;
using System.Diagnostics.CodeAnalysis;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal sealed class InMemoryFeedRepository : IFeedRepository
    {
        private static readonly IDictionary<string, Feed> Data = new Dictionary<string, Feed>(StringComparer.OrdinalIgnoreCase)
        {
            { "the-guardian-uk", new Feed(feedId: "the-guardian-uk", url: "https://www.theguardian.com/uk/rss") }
        };

        public InMemoryFeedRepository() { }

        public InMemoryFeedRepository(params Feed[] feeds)
        {
            foreach (var feed in feeds)
            {
                Data.Add(feed.FeedId, feed);
            }
        }

        public Task<Feed?> GetAsync(string feedId, CancellationToken cancellationToken = default)
        {
            if (Data.TryGetValue(feedId, out var feed))
                return Task.FromResult<Feed?>(feed);

            return Task.FromResult<Feed?>(null);
        }

        public Task UpdateAsync(string feedId, Feed feed, CancellationToken cancellationToken = default)
        {
            Data[feedId] = feed;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string feedId, CancellationToken cancellationToken = default)
        {
            Data.Remove(feedId);
            return Task.CompletedTask;
        }
    }
}
