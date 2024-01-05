using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal sealed class InMemoryChannelPublisher : IChannelPublisher
    {
        private readonly ConcurrentBag<Channel> _publishedChannels = new ConcurrentBag<Channel>();

        public IEnumerable<Channel> PublishedChannels => _publishedChannels;

        public Task PublishChannelUpdatedAsync(Channel channel, CancellationToken cancellationToken = default)
        {
            _publishedChannels.Add(channel);
            return Task.CompletedTask;
        }
    }
}
