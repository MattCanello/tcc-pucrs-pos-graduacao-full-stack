using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface ICreateFeedApp
    {
        Task<Feed> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default);
    }
}
