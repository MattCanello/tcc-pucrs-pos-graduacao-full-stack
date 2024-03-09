using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Policies
{
    public interface IPublishEntryPolicy
    {
        bool ShouldPublish(Feed feed, Entry entry);
    }
}
