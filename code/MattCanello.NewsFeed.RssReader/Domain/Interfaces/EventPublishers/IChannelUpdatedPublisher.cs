using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers
{
    public interface IChannelUpdatedPublisher
    {
        Task PublishAsync(string feedId, Channel channel, CancellationToken cancellationToken = default);
    }
}
