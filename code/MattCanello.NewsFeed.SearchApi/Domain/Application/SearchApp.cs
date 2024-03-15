using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Domain.Application
{
    public sealed class SearchApp : ISearchApp
    {
        private readonly IDocumentSearchRepository _documentSearchRepository;

        public SearchApp(IDocumentSearchRepository documentSearchRepository)
        {
            _documentSearchRepository = documentSearchRepository;
        }

        public async Task<SearchResponse<Document>> SearchAsync(SearchCommand searchCommand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(searchCommand);

            var response = await _documentSearchRepository.SearchAsync(
                searchCommand.Query,
                searchCommand.Paging,
                searchCommand.FeedId,
                cancellationToken);

            return response;
        }
    }
}
