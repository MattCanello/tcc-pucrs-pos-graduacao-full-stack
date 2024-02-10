namespace MattCanello.NewsFeed.CronApi.Domain.Interfaces
{
    public interface ICronPublishApp
    {
        Task<int> PublishSlotAsync(byte slot, CancellationToken cancellationToken = default);
    }
}