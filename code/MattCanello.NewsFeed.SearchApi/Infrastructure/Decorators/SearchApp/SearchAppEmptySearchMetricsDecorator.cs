using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Decorators.SearchApp
{
    public sealed class SearchAppEmptySearchMetricsDecorator : ISearchApp
    {
        private readonly ISearchApp _innerApp;

        public SearchAppEmptySearchMetricsDecorator(ISearchApp innerApp)
        {
            _innerApp = innerApp;
        }

        public async Task<SearchResponse<Document>> SearchAsync(SearchCommand searchCommand, CancellationToken cancellationToken = default)
        {
            var count = Metrics.EmptySearchCount.CreateCounter<int>("empty-search-results-count", "Searched", "Empty search results count");

            var response = await _innerApp.SearchAsync(searchCommand, cancellationToken);

            count.Add(response.IsEmpty ? 1 : 0);

            return response;
        }
    }
}
