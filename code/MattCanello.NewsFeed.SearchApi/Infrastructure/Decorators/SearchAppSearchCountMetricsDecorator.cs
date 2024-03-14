using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;
using MattCanello.NewsFeed.SearchApi.Infrastructure.Telemetry;
using System.Diagnostics;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Decorators
{
    public sealed class SearchAppSearchCountMetricsDecorator : ISearchApp
    {
        private readonly ISearchApp _innerApp;

        public SearchAppSearchCountMetricsDecorator(ISearchApp innerApp)
        {
            _innerApp = innerApp;
        }

        public async Task<SearchResponse<Document>> SearchAsync(SearchCommand searchCommand, CancellationToken cancellationToken = default)
        {
            var count = Metrics.SearchCount.CreateCounter<int>("search-count", "Searches", "Search count");

            var response = await _innerApp.SearchAsync(searchCommand, cancellationToken);

            count.Add(1);

            return response;
        }
    }
}
