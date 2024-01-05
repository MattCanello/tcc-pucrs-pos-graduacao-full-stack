using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Services
{
    public sealed class ChannelPublisher : IChannelPublisher
    {
        public Task PublishChannelUpdatedAsync(Channel channel, CancellationToken cancellationToken = default)
        {
            // TODO: Implementar.
            return Task.CompletedTask;
        }
    }
}
