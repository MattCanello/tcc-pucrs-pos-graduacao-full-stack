using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Domain.Application
{
    public sealed class FeedApp : IFeedApp
    {
        private readonly IDocumentSearchRepository _documentSearchRepository;

        public FeedApp(IDocumentSearchRepository documentSearchRepository)
        {
            _documentSearchRepository = documentSearchRepository;
        }

        public async Task<FeedResponse> GetFeedAsync(GetFeedCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var paging = new Paging(command.Skip, command.Size);

            var documents = await _documentSearchRepository.GetRecentAsync(paging, command.FeedId, cancellationToken);

            return new FeedResponse(documents.Results ?? Array.Empty<Document>(), documents.Total);
        }
    }
}
