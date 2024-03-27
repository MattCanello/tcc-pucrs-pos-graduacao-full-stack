using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IFeedApp
    {
        Task<Feed> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default);
        Task<Feed> UpdateFeedAsync(UpdateFeedCommand command, CancellationToken cancellationToken = default);
    }
}
