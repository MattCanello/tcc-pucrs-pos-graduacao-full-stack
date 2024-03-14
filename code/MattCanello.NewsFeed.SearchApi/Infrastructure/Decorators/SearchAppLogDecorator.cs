using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Decorators
{
    public sealed class SearchAppLogDecorator : ISearchApp
    {
        private readonly ISearchApp _innerApp;
        private readonly ILogger _logger;

        public SearchAppLogDecorator(ISearchApp innerApp, ILogger<SearchAppLogDecorator> logger)
        {
            _innerApp = innerApp;
            _logger = logger;
        }

        public async Task<SearchResponse<Document>> SearchAsync(SearchCommand searchCommand, CancellationToken cancellationToken = default)
        {
            using var scope = _logger.BeginScope(searchCommand);

            _logger.LogInformation("Started searching for '{query}' on '{feedId}'", searchCommand.Query, 
                string.IsNullOrEmpty(searchCommand.FeedId) ? "(all feeds)" : searchCommand.FeedId);

            var response = await _innerApp.SearchAsync(searchCommand, cancellationToken);

            _logger.LogInformation("Search ended with {total} result(s) for '{query}'", response.Total, searchCommand.Query);

            return response;
        }
    }
}
