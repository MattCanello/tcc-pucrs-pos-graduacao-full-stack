using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.CronApi.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Decorators
{
    public sealed class CronPublishAppMetricsDecorator : ICronPublishApp
    {
        private readonly ICronPublishApp _innerApp;

        public CronPublishAppMetricsDecorator(ICronPublishApp innerApp)
        {
            _innerApp = innerApp;
        }

        public async Task<int> PublishSlotAsync(byte slot, CancellationToken cancellationToken = default)
        {
            var count = Metrics.PublishedSlotsCount.CreateCounter<int>("slot-feed-count", "Feed", "Published feeds count");

            using var activity = ActivitySources.CronPublishApp.StartActivity("PublishSlotActivity");

            var response = await _innerApp.PublishSlotAsync(slot, cancellationToken);

            count.Add(response);

            activity?.SetTag("slot", slot);

            return response;
        }
    }
}
