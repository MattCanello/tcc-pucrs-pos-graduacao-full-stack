using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers
{
    public interface IFeedConsumedPublisher
    {
        Task PublishAsync(string feedId, DateTimeOffset consumedDate, Channel channel, CancellationToken cancellationToken = default);
    }
}
