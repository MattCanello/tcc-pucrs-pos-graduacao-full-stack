using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;
using System.Collections.Concurrent;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    internal sealed class InMemoryEntryPublisher : IEntryPublisher
    {
        private readonly ConcurrentBag<Entry> _publishedEntries = new ConcurrentBag<Entry>();

        public IEnumerable<Entry> PublishedEntries => _publishedEntries;

        public Task PublishNewEntryAsync(string feedId, Entry entry, CancellationToken cancellationToken = default)
        {
            _publishedEntries.Add(entry);
            return Task.CompletedTask;
        }
    }
}
