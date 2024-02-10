using MattCanello.NewsFeed.CronApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Tests.Fakes
{
    public sealed class FakeCronFeedEnqueuer : ICronFeedEnqueuer
    {
        private readonly Queue<string> _queue;

        public FakeCronFeedEnqueuer(params string[] alreadyEnqueued)
        {
            _queue = new Queue<string>(alreadyEnqueued);
        }

        public IReadOnlyCollection<string> Queue => _queue;

        public Task EnqueueFeedToProcessAsync(string feedId, CancellationToken cancellationToken = default)
        {
            _queue.Enqueue(feedId);
            return Task.CompletedTask;
        }
    }
}
