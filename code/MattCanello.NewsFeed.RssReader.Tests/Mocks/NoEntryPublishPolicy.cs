using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Policies;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    sealed class NoEntryPublishPolicy : IPublishEntryPolicy
    {
        public bool ShouldPublish(Feed feed, Entry entry)
        {
            return true;
        }
    }
}
