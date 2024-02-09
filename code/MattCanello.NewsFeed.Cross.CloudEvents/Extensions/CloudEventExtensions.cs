using CloudNative.CloudEvents;

namespace MattCanello.NewsFeed.Cross.CloudEvents.Extensions
{
    public static class CloudEventExtensions
    {
        public static string? GetFeedId(this CloudEvent cloudEvent)
        {
            if (cloudEvent is null)
                return null;

            var feedIdAttr = cloudEvent.GetAttribute("feedid");
            if (feedIdAttr is null)
                return null;

            var attributes = new Dictionary<CloudEventAttribute, object>(cloudEvent.GetPopulatedAttributes());
            if (attributes.Count == 0)
                return null;

            if (!attributes.TryGetValue(feedIdAttr, out var feedId))
                return null;

            return feedId as string;
        }
    }
}
