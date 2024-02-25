using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Domain.Applications
{
    public sealed class CronPublishApp : ICronPublishApp
    {
        private readonly IFeedRepository _feedRepository;
        private readonly ICronFeedEnqueuer _cronFeedEnqueuer;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CronPublishApp(IFeedRepository feedRepository, ICronFeedEnqueuer cronFeedEnqueuer, IDateTimeProvider dateTimeProvider)
        {
            _feedRepository = feedRepository;
            _cronFeedEnqueuer = cronFeedEnqueuer;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> PublishSlotAsync(byte slot, CancellationToken cancellationToken = default)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);

            var executionDate = _dateTimeProvider.GetUtcNow();
            var feedIds = await _feedRepository.GetFeedIdsAsync(slot, cancellationToken);

            if (feedIds is null || feedIds.Count == 0)
                return 0;

            foreach (var feedId in feedIds)
            {
                await EnqueueAndUpdateFeedAsync(slot, feedId, executionDate, cancellationToken);
            }

            return feedIds.Count;
        }

        private async Task EnqueueAndUpdateFeedAsync(byte slot, string feedId, DateTimeOffset executionDate, CancellationToken cancellationToken = default)
        {
            await _cronFeedEnqueuer.EnqueueFeedToProcessAsync(feedId, cancellationToken);
            await _feedRepository.UpdateLastExecutionDateAsync(slot, feedId, executionDate, cancellationToken);
        }
    }
}
