using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Domain.Decorators
{
    public sealed class SearchAppEmptyQueryDecorator : ISearchApp
    {
        private readonly ISearchApp _searchApp;
        private readonly IDocumentSearchRepository _documentSearchRepository;

        public SearchAppEmptyQueryDecorator(ISearchApp searchApp, IDocumentSearchRepository documentSearchRepository)
        {
            _searchApp = searchApp;
            _documentSearchRepository = documentSearchRepository;
        }

        public async Task<SearchResponse<Document>> SearchAsync(SearchCommand searchCommand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(searchCommand);

            if (!string.IsNullOrEmpty(searchCommand.Query))
                return await _searchApp.SearchAsync(searchCommand, cancellationToken);

            return await _documentSearchRepository
                .GetRecentAsync(searchCommand.Paging ?? new Paging(), searchCommand.FeedId, searchCommand.ChannelId, cancellationToken);
        }
    }
}
