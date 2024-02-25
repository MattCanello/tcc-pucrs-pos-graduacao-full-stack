using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Application;
using MattCanello.NewsFeed.RssReader.Domain.Responses;
using MattCanello.NewsFeed.RssReader.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Decorators
{
    public sealed class RssAppMetricsDecorator : IRssApp
    {
        private readonly IRssApp _rssApp;

        public RssAppMetricsDecorator(IRssApp rssApp)
        {
            _rssApp = rssApp;
        }

        public async Task<ProcessRssResponse> ProcessFeedAsync(string feedId, CancellationToken cancellationToken = default)
        {
            var count = Metrics.PublishedEntriesCount.CreateCounter<int>("entries-count", "Entry", "Published entries count'");

            using var activity = ActivitySources.RssApp.StartActivity("ProcessFeedActivity");

            var response = await _rssApp.ProcessFeedAsync(feedId, cancellationToken);

            count.Add(response.PublishedEntriesCount);

            activity?.SetTag("feedId", feedId);

            return response;
        }
    }
}
