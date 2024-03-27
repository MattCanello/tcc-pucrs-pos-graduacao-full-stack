using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IChannelApp
    {
        Task<Channel> CreateAsync(CreateChannelCommand command, CancellationToken cancellationToken = default);
        Task<Channel> UpdateAsync(UpdateChannelCommand command, CancellationToken cancellationToken = default);
    }
}
