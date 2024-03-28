using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Domain.Responses;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IChannelRepository
    {
        Task<Channel?> GetByIdAsync(string channelId, CancellationToken cancellationToken = default);

        Task<Channel> CreateAsync(Channel channel, CancellationToken cancellationToken = default);

        Task<Channel> UpdateAsync(Channel channel, CancellationToken cancellationToken = default);

        Task<QueryResponse<Channel>> QueryAsync(QueryCommand command, CancellationToken cancellationToken = default);
    }
}
