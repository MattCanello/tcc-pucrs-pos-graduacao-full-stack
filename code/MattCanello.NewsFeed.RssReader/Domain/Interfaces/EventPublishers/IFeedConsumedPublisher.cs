using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers
{
    public interface IFeedConsumedPublisher
    {
        Task PublishAsync(string feedId, Channel channel, CancellationToken cancellationToken = default);
    }
}
