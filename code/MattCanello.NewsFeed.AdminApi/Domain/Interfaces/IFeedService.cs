using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IFeedService
    {
        Task<Feed> UpdateFeedAsync(string feedId, ChannelData channelData, CancellationToken cancellationToken = default);
    }
}
