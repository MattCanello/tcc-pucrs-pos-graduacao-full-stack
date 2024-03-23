using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishFeedCreatedEventAsync(Feed feed, CancellationToken cancellationToken = default);
    }
}
