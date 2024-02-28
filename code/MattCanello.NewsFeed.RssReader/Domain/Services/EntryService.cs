using MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Services;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Services
{
    public sealed class EntryService : IEntryService
    {
        private readonly IEntryFactory _entryFactory;
        private readonly INewEntryFoundPublisher _newEntryFoundPublisher;

        public EntryService(IEntryFactory entryFactory, INewEntryFoundPublisher newEntryFoundPublisher)
        {
            _entryFactory = entryFactory;
            _newEntryFoundPublisher = newEntryFoundPublisher;
        }

        public async Task<int> ProcessEntriesFromRSSAsync(string feedId, SyndicationFeed syndicationFeed, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(syndicationFeed);

            var publishTasks = new List<Task>();
            foreach (var entry in _entryFactory.FromRSS(syndicationFeed))
            {
                var publishEntryTask = _newEntryFoundPublisher.PublishAsync(feedId, entry, cancellationToken);
                publishTasks.Add(publishEntryTask);
            }

            await Task.WhenAll(publishTasks);
            return publishTasks.Count;
        }
    }
}
