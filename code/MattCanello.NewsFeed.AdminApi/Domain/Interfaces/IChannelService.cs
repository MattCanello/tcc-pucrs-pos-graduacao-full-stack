using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IChannelService
    {
        Task<Channel> GetOrCreateAsync(string channelId, CancellationToken cancellationToken = default);

        Task<Channel> UpdateChannelAsync(string channelId, RssChannel rssChannel, CancellationToken cancellationToken = default);
    }
}
