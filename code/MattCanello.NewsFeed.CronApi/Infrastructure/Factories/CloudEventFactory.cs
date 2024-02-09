using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Extensions;
using MattCanello.NewsFeed.CronApi.Infrastructure.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Factories
{
    public sealed class CloudEventFactory : ICloudEventFactory
    {
        public CloudEvent CreateProcessRssEvent(string feedId)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V1_0)
            {
                Data = new { feedId },
                Type = "mattcanello.newsfeed.processrssfeed",
                Time = DateTimeOffset.UtcNow,
                Id = feedId,
                Subject = feedId,
                Source = new Uri($"/cron-api/{feedId}", UriKind.Relative)
            };

            cloudEvent.SetPartitionKey(feedId);
            cloudEvent.SetAttributeFromString("feedid", feedId);

            return cloudEvent;
        }
    }
}
