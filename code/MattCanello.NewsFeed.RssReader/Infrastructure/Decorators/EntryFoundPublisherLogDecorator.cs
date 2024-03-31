using MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Decorators
{
    public sealed class EntryFoundPublisherLogDecorator : INewEntryFoundPublisher
    {
        private readonly ILogger _logger;
        private readonly INewEntryFoundPublisher _innerPublisher;

        public EntryFoundPublisherLogDecorator(ILogger<EntryFoundPublisherLogDecorator> logger, INewEntryFoundPublisher innerPublisher)
        {
            _logger = logger;
            _innerPublisher = innerPublisher;
        }

        public async Task PublishAsync(string feedId, Entry entry, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope(new { feedId, entry }))
            {
                _logger.LogTrace("Starting publishing '{entry.Id}' to feed '{feedId}'", entry.Id, feedId);

                await _innerPublisher.PublishAsync(feedId, entry, cancellationToken);

                _logger.LogInformation("Published '{entry.Id}' on feed '{feedId}'", entry.Id, feedId);
            }
        }
    }
}
