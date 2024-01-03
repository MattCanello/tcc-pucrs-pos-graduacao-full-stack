using MattCanello.NewsFeed.RssReader.Interfaces;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Services
{
    public sealed class EntryService : IEntryService
    {
        private readonly IEntryReader _entryReader;
        private readonly IEntryPublisher _entryPublisher;

        public EntryService(IEntryReader entryReader, IEntryPublisher entryPublisher)
        {
            _entryReader = entryReader;
            _entryPublisher = entryPublisher;
        }

        public async Task ProcessEntriesFromRSSAsync(string feedId, SyndicationFeed syndicationFeed, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(syndicationFeed);

            var publishTasks = new List<Task>();
            foreach (var entry in _entryReader.FromRSS(syndicationFeed))
            {
                var publishEntryTask = _entryPublisher.PublishNewEntryAsync(feedId, entry, cancellationToken);
                publishTasks.Add(publishEntryTask);
            }

            await Task.WhenAll(publishTasks);
        }
    }
}
