using MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using System.Collections.Concurrent;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    internal sealed class InMemoryEntryPublisher : INewEntryFoundPublisher
    {
        private readonly ConcurrentBag<Entry> _publishedEntries = new ConcurrentBag<Entry>();

        public IEnumerable<Entry> PublishedEntries => _publishedEntries;

        public Task PublishAsync(string feedId, Entry entry, CancellationToken cancellationToken = default)
        {
            _publishedEntries.Add(entry);
            return Task.CompletedTask;
        }
    }
}
