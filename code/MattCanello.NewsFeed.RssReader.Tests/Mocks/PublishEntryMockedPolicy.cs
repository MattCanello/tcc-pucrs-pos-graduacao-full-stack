using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Policies;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    sealed class PublishEntryMockedPolicy : IPublishEntryPolicy
    {
        private readonly Func<Feed, Entry, bool> _shouldPublish;

        public PublishEntryMockedPolicy(bool shouldPublish)
            : this((feed, entry) => shouldPublish) { }

        public PublishEntryMockedPolicy(Func<Feed, Entry, bool> shouldPublish) 
            => _shouldPublish = shouldPublish ?? throw new ArgumentNullException(nameof(shouldPublish));

        public bool ShouldPublish(Feed feed, Entry entry) => _shouldPublish(feed, entry);
    }
}
