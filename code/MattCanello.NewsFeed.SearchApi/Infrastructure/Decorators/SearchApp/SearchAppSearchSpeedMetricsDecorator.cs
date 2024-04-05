using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Decorators.SearchApp
{
    public sealed class SearchAppSearchSpeedMetricsDecorator : ISearchApp
    {
        private readonly ISearchApp _innerApp;

        public SearchAppSearchSpeedMetricsDecorator(ISearchApp innerApp)
        {
            _innerApp = innerApp;
        }

        public async Task<SearchResponse<Document>> SearchAsync(SearchCommand searchCommand, CancellationToken cancellationToken = default)
        {
            var histogram = Metrics.SearchSpeed.CreateHistogram<double>("search-speed", "ms", "Search speed (in milliseconds)");

            using var activity = ActivitySources.SearchApp.StartActivity("SearchActivity");

            var response = await _innerApp.SearchAsync(searchCommand, cancellationToken);

            activity?
                .SetTag("isEmpty", response.IsEmpty)
                .SetTag("totalCount", response.Total);

            activity?.Stop();

            histogram.Record(activity?.Duration.TotalMilliseconds ?? 0d);

            return response;
        }
    }
}
