using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Extensions;
using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Factories
{
    public sealed class CloudEventFactory : ICloudEventFactory
    {
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

        public CloudEvent CreateChannelUpdatedEvent(string channelId, Channel channel)
        {
            ArgumentNullException.ThrowIfNull(channelId);
            ArgumentNullException.ThrowIfNull(channel);

            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V1_0)
            {
                Data = channel,
                Type = "mattcanello.newsfeed.channelupdated",
                Time = DateTimeOffset.UtcNow,
                Id = channelId,
                Subject = channelId,
                Source = new Uri($"/rss-reader/{channelId}", UriKind.Relative)
            };

            cloudEvent.SetPartitionKey(channelId);
            cloudEvent.SetAttributeFromString("channelid", channelId);

            return cloudEvent;
        }

    }
}
