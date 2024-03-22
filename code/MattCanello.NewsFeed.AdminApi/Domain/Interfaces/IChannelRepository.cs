using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IChannelRepository
    {
        Task<Channel?> GetByIdAsync(string channelId, CancellationToken cancellationToken = default);

        Task<Channel> CreateAsync(Channel channel, CancellationToken cancellationToken = default);
    }
}
