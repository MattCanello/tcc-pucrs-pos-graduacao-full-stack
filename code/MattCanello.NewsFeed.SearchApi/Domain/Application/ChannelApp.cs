using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Domain.Application
{
    public sealed class ChannelApp : IChannelApp
    {
        private readonly IDocumentSearchRepository _documentSearchRepository;

        public ChannelApp(IDocumentSearchRepository documentSearchRepository) 
            => _documentSearchRepository = documentSearchRepository;

        public async Task<SearchResponse<Document>> GetDocumentsAsync(GetChannelDocumentsCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var paging = new Paging(command.Skip, command.Size);

            var documents = await _documentSearchRepository.GetRecentByChannelAsync(paging, command.ChannelId, cancellationToken);

            return documents;
        }
    }
}
