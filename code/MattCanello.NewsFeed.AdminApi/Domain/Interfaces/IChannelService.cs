using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IChannelService
    {
        Task<Channel> GetOrCreateAsync(string channelId, CancellationToken cancellationToken);
    }
}
