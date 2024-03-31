using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Evalulators;
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
        private readonly IMostRecentPublishDateEvaluator _mostRecentPublishDateEvaluator;

        public EntryService(
            IEntryFactory entryFactory, 
            INewEntryFoundPublisher newEntryFoundPublisher, 
            IPublishEntryPolicy publishEntryPolicy, 
            IMostRecentPublishDateEvaluator mostRecentPublishDateEvaluator)
        {
            _entryFactory = entryFactory;
            _newEntryFoundPublisher = newEntryFoundPublisher;
            _publishEntryPolicy = publishEntryPolicy;
            _mostRecentPublishDateEvaluator = mostRecentPublishDateEvaluator;
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

                var publishEntryTask = _newEntryFoundPublisher.PublishAsync(feed.FeedId, feed.ChannelId, entry, cancellationToken);
                publishTasks.Add(publishEntryTask);

                mostRecentPublishDate = _mostRecentPublishDateEvaluator.Evaluate(mostRecentPublishDate, entry.PublishDate);
            }

            await Task.WhenAll(publishTasks);

            return new PublishRssEntriesResponse(publishTasks.Count, mostRecentPublishDate);
        }
    }
}
