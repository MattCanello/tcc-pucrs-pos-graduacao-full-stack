using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal sealed class InMemoryChannelPublisher : IChannelPublisher
    {
        private readonly ConcurrentDictionary<string, Channel> _publishedChannels = new ConcurrentDictionary<string, Channel>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<Channel> PublishedChannels => _publishedChannels.Values;
        public IEnumerable<string> PublishedChannelIds => _publishedChannels.Keys;

        public Task PublishChannelUpdatedAsync(string channelId, Channel channel, CancellationToken cancellationToken = default)
        {
            _publishedChannels.TryAdd(channelId, channel);
            return Task.CompletedTask;
        }
    }
}
