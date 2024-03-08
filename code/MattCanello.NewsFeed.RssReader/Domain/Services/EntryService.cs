using MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Services;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Domain.Responses;
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

        public async Task<PublishRssEntriesResponse> ProcessEntriesFromRSSAsync(string feedId, SyndicationFeed syndicationFeed, DateTimeOffset? lastPublishedEntryDate, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(syndicationFeed);

            var publishTasks = new List<Task>();

            DateTimeOffset? mostRecentPublishDate = null;

            foreach (var entry in _entryFactory.FromRSS(syndicationFeed))
            {
                if (!ShouldPublish(entry, lastPublishedEntryDate))
                    continue;

                var publishEntryTask = _newEntryFoundPublisher.PublishAsync(feedId, entry, cancellationToken);
                publishTasks.Add(publishEntryTask);

                mostRecentPublishDate = EvaluateMostRecentPublishDate(mostRecentPublishDate, entry.PublishDate);
            }

            await Task.WhenAll(publishTasks);

            return new PublishRssEntriesResponse(publishTasks.Count, mostRecentPublishDate);
        }

        private static bool ShouldPublish(Entry? entry, DateTimeOffset? lastPublishedEntryDate)
        {
            if (entry is null)
                return false;

            if (lastPublishedEntryDate is null)
                return true;

            return entry.PublishDate >= lastPublishedEntryDate;
        }

        private static DateTimeOffset EvaluateMostRecentPublishDate(DateTimeOffset? current, DateTimeOffset entryPublishDate)
        {
            if (current is null)
                return entryPublishDate;

            if (current > entryPublishDate)
                return current.Value;

            return entryPublishDate;
        }
    }
}
