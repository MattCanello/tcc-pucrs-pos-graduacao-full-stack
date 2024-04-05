using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Mocks
{
    sealed class MockedSearchClient : ISearchClient
    {
        private readonly Func<string, string, SearchDocument?> _getDocumentByIdFunc;
        private readonly Func<string?, int?, SearchResponse<SearchDocument>> _getRecentFunc;
        private readonly Func<SearchCommand, SearchResponse<SearchDocument>> _searchFunc;

        public MockedSearchClient(Func<string, string, SearchDocument?> getDocumentByIdFunc)
        {
            _getDocumentByIdFunc = getDocumentByIdFunc;
            _getRecentFunc = (channelId, size) => throw new NotImplementedException();
            _searchFunc = (cmd) => throw new NotImplementedException();
        }

        public Task<SearchDocument?> GetDocumentByIdAsync(string feedId, string id, CancellationToken cancellationToken = default)
        {
            var document = _getDocumentByIdFunc(feedId, id);

            return Task.FromResult(document);
        }

        public Task<SearchResponse<SearchDocument>> GetRecentAsync(string? channelId = null, int? size = null, CancellationToken cancellationToken = default)
        {
            var getRecentResult = _getRecentFunc(channelId, size);

            return Task.FromResult(getRecentResult);
        }

        public Task<SearchResponse<SearchDocument>> SearchAsync(SearchCommand command, CancellationToken cancellationToken = default)
        {
            var getRecentResult = _searchFunc(command);

            return Task.FromResult(getRecentResult);
        }
    }
}
