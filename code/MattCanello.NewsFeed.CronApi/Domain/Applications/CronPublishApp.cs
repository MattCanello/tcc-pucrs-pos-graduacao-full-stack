using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Domain.Applications
{
    public sealed class CronPublishApp : ICronPublishApp
    {
        private readonly IFeedRepository _feedRepository;
        private readonly ICronFeedEnqueuer _cronFeedEnqueuer;

        public CronPublishApp(IFeedRepository feedRepository, ICronFeedEnqueuer cronFeedEnqueuer)
        {
            _feedRepository = feedRepository;
            _cronFeedEnqueuer = cronFeedEnqueuer;
        }

        public async Task<int> PublishSlotAsync(byte slot, CancellationToken cancellationToken = default)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);

            var executionDate = DateTimeOffset.UtcNow;
            var feedIds = await _feedRepository.GetFeedIdsAsync(slot, cancellationToken);

            if (feedIds is null || feedIds.Count == 0)
                return 0;

            var enqueueTasks = new List<Task>(capacity: feedIds.Count);

            foreach (var feedId in feedIds)
            {
                var task = EnqueueAndUpdateFeedAsync(slot, feedId, executionDate, cancellationToken);

                enqueueTasks.Add(task);
            }

            await Task.WhenAll(enqueueTasks);

            return feedIds.Count;
        }

        private async Task EnqueueAndUpdateFeedAsync(byte slot, string feedId, DateTimeOffset executionDate, CancellationToken cancellationToken = default)
        {
            await _cronFeedEnqueuer.EnqueueFeedToProcessAsync(feedId, cancellationToken);
            await _feedRepository.UpdateLastExecutionDateAsync(slot, feedId, executionDate, cancellationToken);
        }
    }
}
