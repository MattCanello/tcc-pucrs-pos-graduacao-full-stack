using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface IChannelRepository
    {
        Task<IEnumerable<Channel>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
