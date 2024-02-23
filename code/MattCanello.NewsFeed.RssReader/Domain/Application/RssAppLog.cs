using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Application;

namespace MattCanello.NewsFeed.RssReader.Domain.Application
{
    public sealed class RssAppLog : IRssApp
    {
        private readonly ILogger _logger;
        private readonly RssApp _rssApp;

        public RssAppLog(ILogger<RssAppLog> logger, RssApp rssApp)
        {
            _logger = logger;
            _rssApp = rssApp;
        }

        public async Task ProcessFeedAsync(string feedId, CancellationToken cancellationToken = default)
        {
            using var state = _logger.BeginScope(new { feedId });

            _logger.LogInformation("Starting processing feed '{feedId}'", feedId);

            await _rssApp.ProcessFeedAsync(feedId, cancellationToken);

            _logger.LogInformation("Finished processing feed '{feedId}'", feedId);
        }
    }
}
