using MattCanello.NewsFeed.AdminApi.Domain.Models;
using static MattCanello.NewsFeed.AdminApi.Domain.Commands.UpdateChannelCommand;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IChannelService
    {
        Task<Channel> GetOrCreateAsync(string channelId, CancellationToken cancellationToken = default);

        Task<Channel> UpdateChannelAsync(string channelId, ChannelData channelData, CancellationToken cancellationToken = default);
    }
}
