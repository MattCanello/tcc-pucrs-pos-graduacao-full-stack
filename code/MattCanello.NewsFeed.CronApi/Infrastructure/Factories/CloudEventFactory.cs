using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Extensions;
using MattCanello.NewsFeed.CronApi.Infrastructure.Interfaces;
using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;
using MattCanello.NewsFeed.Cross.CloudEvents.Extensions;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Factories
{
    public sealed class CloudEventFactory : ICloudEventFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CloudEventFactory(IDateTimeProvider dateTimeProvider) => _dateTimeProvider = dateTimeProvider;

        public CloudEvent CreateProcessRssEvent(string feedId)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V1_0)
            {
                Data = new { feedId },
                Type = "mattcanello.newsfeed.processrssfeed",
                Time = _dateTimeProvider.GetUtcNow(),
                Id = feedId,
                Subject = feedId,
                Source = new Uri($"/cron-api/{feedId}", UriKind.Relative)
            };

            cloudEvent.SetPartitionKey(feedId);
            cloudEvent.SetFeedId(feedId);

            return cloudEvent;
        }
    }
}
