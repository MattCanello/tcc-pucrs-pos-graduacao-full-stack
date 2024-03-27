using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IChannelService
    {
        Task<Channel> GetOrCreateAsync(string channelId, RssData? data = null, CancellationToken cancellationToken = default);

        Task<Channel> AppendDataToChannelAsync(string channelId, RssData data, CancellationToken cancellationToken = default);
    }
}
