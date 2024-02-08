using MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal sealed class InMemoryFeedConsumedPublisher : IFeedConsumedPublisher
    {
        private readonly ConcurrentDictionary<string, Channel> _publishedChannels = new ConcurrentDictionary<string, Channel>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<Channel> PublishedChannels => _publishedChannels.Values;
        public IEnumerable<string> PublishedFeedIds => _publishedChannels.Keys;

        public Task PublishAsync(string feedId, DateTimeOffset consumedDate, Channel channel, CancellationToken cancellationToken = default)
        {
            _publishedChannels.TryAdd(feedId, channel);
            return Task.CompletedTask;
        }
    }
}
