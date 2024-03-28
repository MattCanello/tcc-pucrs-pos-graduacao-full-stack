using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishFeedCreatedEventAsync(FeedWithChannel feed, CancellationToken cancellationToken = default);
    }
}
