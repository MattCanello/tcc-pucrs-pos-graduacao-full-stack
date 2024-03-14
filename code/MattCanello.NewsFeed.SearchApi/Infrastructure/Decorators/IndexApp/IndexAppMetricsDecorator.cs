using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Decorators.IndexApp
{
    public sealed class IndexAppMetricsDecorator : IIndexApp
    {
        private readonly IIndexApp _innerApp;

        public IndexAppMetricsDecorator(IIndexApp indexApp)
        {
            _innerApp = indexApp;
        }

        public async Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            var count = Metrics.IndexedDocuments.CreateCounter<int>("indexed-documents-count", "Entries", "Indexed documents count");

            using var activity = ActivitySources.IndexApp.StartActivity("IndexActivity");

            var response = await _innerApp.IndexAsync(command, cancellationToken);

            count.Add(1);

            activity?.SetTag("feedId", command.FeedId);

            return response;
        }
    }
}
