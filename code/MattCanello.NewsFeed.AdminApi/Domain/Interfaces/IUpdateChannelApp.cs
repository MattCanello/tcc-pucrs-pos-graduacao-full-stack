using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Interfaces
{
    public interface IUpdateChannelApp
    {
        Task<Channel> UpdateChannelAsync(UpdateChannelCommand command, CancellationToken cancellationToken = default);
    }
}
