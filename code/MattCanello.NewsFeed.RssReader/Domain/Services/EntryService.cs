using MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Policies;
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
        private readonly IPublishEntryPolicy _publishEntryPolicy;

        public EntryService(IEntryFactory entryFactory, INewEntryFoundPublisher newEntryFoundPublisher, IPublishEntryPolicy publishEntryPolicy)
        {
            _entryFactory = entryFactory;
            _newEntryFoundPublisher = newEntryFoundPublisher;
            _publishEntryPolicy = publishEntryPolicy;
        }

        public async Task<PublishRssEntriesResponse> ProcessEntriesFromRSSAsync(Feed feed, SyndicationFeed syndicationFeed, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(syndicationFeed);

            var publishTasks = new List<Task>();

            DateTimeOffset? mostRecentPublishDate = null;

            foreach (var entry in _entryFactory.FromRSS(syndicationFeed))
            {
                if (!_publishEntryPolicy.ShouldPublish(feed, entry))
                    continue;

                var publishEntryTask = _newEntryFoundPublisher.PublishAsync(feed.FeedId, entry, cancellationToken);
                publishTasks.Add(publishEntryTask);

                mostRecentPublishDate = EvaluateMostRecentPublishDate(mostRecentPublishDate, entry.PublishDate);
            }

            await Task.WhenAll(publishTasks);

            return new PublishRssEntriesResponse(publishTasks.Count, mostRecentPublishDate);
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
