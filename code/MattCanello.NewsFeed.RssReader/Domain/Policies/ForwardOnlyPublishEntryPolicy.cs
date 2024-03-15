using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Policies;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Policies
{
    public sealed class ForwardOnlyPublishEntryPolicy : IPublishEntryPolicy
    {
        public bool ShouldPublish(Feed feed, Entry entry)
        {
            ArgumentNullException.ThrowIfNull(feed);

            if (entry is null)
                return false;

            if (feed.LastPublishedEntryDate is null)
                return true;

            return entry.PublishDate > feed.LastPublishedEntryDate;
        }
    }
}
