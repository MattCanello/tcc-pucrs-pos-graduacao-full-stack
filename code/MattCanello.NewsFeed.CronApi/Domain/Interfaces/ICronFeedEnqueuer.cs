namespace MattCanello.NewsFeed.CronApi.Domain.Interfaces
{
    public interface ICronFeedEnqueuer
    {
        Task EnqueueFeedToProcessAsync(string feedId, CancellationToken cancellationToken = default);
    }
}
