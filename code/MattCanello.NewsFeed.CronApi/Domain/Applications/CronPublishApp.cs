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

            var feedIds = await _feedRepository.GetFeedIdsAsync(slot, cancellationToken);

            if (feedIds is null || feedIds.Count == 0)
                return 0;

            var enqueueTasks = new List<Task>(capacity: feedIds.Count);

            foreach (var feedId in feedIds)
            {
                var enqueueTask = _cronFeedEnqueuer.EnqueueFeedToProcessAsync(feedId, cancellationToken);
                enqueueTasks.Add(enqueueTask);
            }

            await Task.WhenAll(enqueueTasks);

            return feedIds.Count;
        }
    }
}
