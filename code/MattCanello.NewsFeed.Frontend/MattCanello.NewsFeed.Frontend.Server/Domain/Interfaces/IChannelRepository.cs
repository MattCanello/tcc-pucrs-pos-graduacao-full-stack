using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface IChannelRepository
    {
        Task<IReadOnlyList<Channel>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
