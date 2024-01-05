using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Extensions;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Factories;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Providers;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Factories
{
    public sealed class CloudEventFactory : ICloudEventFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CloudEventFactory(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public CloudEvent CreateNewEntryEvent(string feedId, Entry entry)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(entry);

            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V1_0)
            {
                Data = entry,
                Type = "mattcanello.newsfeed.newentry",
                Time = entry.PublishDate,
                Id = entry.Id,
                Subject = entry.Id,
                Source = new Uri($"/rss-reader/{feedId}", UriKind.Relative)
            };

            cloudEvent.SetPartitionKey(feedId);
            cloudEvent.SetAttributeFromString("feedid", feedId);

            return cloudEvent;
        }

        public CloudEvent CreateChannelUpdatedEvent(string feedId, Channel channel)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(channel);

            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V1_0)
            {
                Data = channel,
                Type = "mattcanello.newsfeed.channelupdated",
                Time = _dateTimeProvider.GetUtcNow(),
                Id = feedId,
                Subject = feedId,
                Source = new Uri($"/rss-reader/{feedId}", UriKind.Relative)
            };

            cloudEvent.SetPartitionKey(feedId);
            cloudEvent.SetAttributeFromString("feedid", feedId);

            return cloudEvent;
        }
    }
}
