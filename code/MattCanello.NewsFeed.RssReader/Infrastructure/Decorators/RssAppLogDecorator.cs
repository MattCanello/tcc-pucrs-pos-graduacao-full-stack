using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Application;
using MattCanello.NewsFeed.RssReader.Domain.Responses;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Decorators
{
    public sealed class RssAppLogDecorator : IRssApp
    {
        private readonly ILogger _logger;
        private readonly IRssApp _rssApp;

        public RssAppLogDecorator(IRssApp rssApp, ILogger<RssAppLogDecorator> logger)
        {
            _rssApp = rssApp;
            _logger = logger;
        }

        public async Task<ProcessRssResponse> ProcessFeedAsync(string feedId, CancellationToken cancellationToken = default)
        {
            using var state = _logger.BeginScope(new { feedId });

            _logger.LogInformation("Starting processing feed '{feedId}'", feedId);

            var response = await _rssApp.ProcessFeedAsync(feedId, cancellationToken);

            _logger.LogInformation("Finished processing feed '{feedId}' with {response.PublishedEntriesCount} entries published", feedId, response.PublishedEntriesCount);

            return response;
        }
    }
}
