using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Infrastructure
{
    public sealed class InMemoryFeedRepository : IFeedRepository
    {
        private static readonly IDictionary<string, Feed> Data = new Dictionary<string, Feed>(StringComparer.OrdinalIgnoreCase)
        {
            { "the-guardian-uk", new Feed(channelId: "the-guardian", feedId: "the-guardian-uk", url: "https://www.theguardian.com/uk/rss") }
        };

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
    }
}
