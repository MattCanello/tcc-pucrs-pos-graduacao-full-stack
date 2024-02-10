using MattCanello.NewsFeed.CronApi.Domain.Models;

namespace MattCanello.NewsFeed.CronApi.Domain.Interfaces
{
    public interface ISlotRepository
    {
        Task<byte> GetCurrentSlotAsync(CancellationToken cancellationToken = default);
        Task SetCurrentSlotAsync(byte slot, CancellationToken cancellationToken = default);
    }
}
