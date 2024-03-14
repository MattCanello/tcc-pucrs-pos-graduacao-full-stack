using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using DocumentSearchResponse = MattCanello.NewsFeed.SearchApi.Domain.Responses.SearchResponse<MattCanello.NewsFeed.SearchApi.Domain.Models.Document>;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Application
{
    public sealed class ElasticSearchSearchApp : ISearchApp
    {
        private readonly IDocumentSearchRepository _documentSearchRepository;

        public ElasticSearchSearchApp(IDocumentSearchRepository documentSearchRepository)
        {
            _documentSearchRepository = documentSearchRepository;
        }

        public async Task<DocumentSearchResponse> SearchAsync(SearchCommand searchCommand, CancellationToken cancellationToken = default)
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
