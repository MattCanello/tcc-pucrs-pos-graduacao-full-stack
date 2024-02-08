using MattCanello.NewsFeed.CronApi.Domain.Models;

namespace MattCanello.NewsFeed.CronApi.Domain.Interfaces
{
    public interface ISlotRepository
    {
        Task<byte> GetCurrentSlotAsync();
        Task SetCurrentSlotAsync(byte slot);
    }
}
