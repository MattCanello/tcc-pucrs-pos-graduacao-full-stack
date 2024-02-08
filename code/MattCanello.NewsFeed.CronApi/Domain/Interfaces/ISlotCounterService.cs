namespace MattCanello.NewsFeed.CronApi.Domain.Interfaces
{
    public interface ISlotCounterService
    {
        Task<byte> GetNextSlotAsync();
    }
}