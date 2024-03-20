using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Domain.Decorators
{
    public sealed class IndexAppEventPublisherDecorator : IIndexApp
    {
        private readonly IIndexApp _indexApp;
        private readonly IIndexedDocumentPublisher _indexedDocumentPublisher;

        public IndexAppEventPublisherDecorator(IIndexApp indexApp, IIndexedDocumentPublisher indexedDocumentPublisher)
        {
            _indexApp = indexApp;
            _indexedDocumentPublisher = indexedDocumentPublisher;
        }

        public async Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            var documentId = await _indexApp.IndexAsync(command, cancellationToken);

            var document = new Document(documentId, command.FeedId!, command.Entry!);

            await _indexedDocumentPublisher.PublishAsync(document, cancellationToken);

            return documentId;
        }
    }
}
