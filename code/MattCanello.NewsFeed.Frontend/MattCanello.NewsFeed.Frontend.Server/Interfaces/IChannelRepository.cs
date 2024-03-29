using MattCanello.NewsFeed.Frontend.Server.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Interfaces
{
    public interface IChannelRepository
    {
        Task<IEnumerable<Channel>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
