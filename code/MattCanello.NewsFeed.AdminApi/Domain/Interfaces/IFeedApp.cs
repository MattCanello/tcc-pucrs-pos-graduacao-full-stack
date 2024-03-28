using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IFeedApp
    {
        Task<FeedWithChannel> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default);
        Task<FeedWithChannel> UpdateFeedAsync(UpdateFeedCommand command, CancellationToken cancellationToken = default);
    }
}
