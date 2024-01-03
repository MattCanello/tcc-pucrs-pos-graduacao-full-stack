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
            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V1_0)
            {
                Data = entry,
                Type = "mattcanello.newsfeed.newentry",
                Time = entry.PublishDate,
                Id = entry.Id,
                Subject = entry.Id,
                Source = new Uri($"/rss-reader/{feedId}")
            };

            cloudEvent.SetPartitionKey(feedId);
            cloudEvent.SetAttributeFromString("feedid", feedId);

            return cloudEvent;
        }
    }
}
