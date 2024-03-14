using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.SearchApi.Domain.Policies
{
    public sealed class PreventDuplicateEntryIndexingPolicy : IEntryIndexPolicy
    {
        private readonly IDocumentSearchRepository _documentSearchRepository;

        public PreventDuplicateEntryIndexingPolicy(IDocumentSearchRepository documentSearchRepository)
        {
            _documentSearchRepository = documentSearchRepository;
        }

        public async Task EvaluateAsync(IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var entryId = command.Entry?.Id;
            var feedId = command.FeedId;

            if (string.IsNullOrEmpty(entryId) || string.IsNullOrEmpty(feedId))
                return;

            var findResult = await _documentSearchRepository.FindByIdAsync(entryId, feedId, cancellationToken);

            if (findResult.IsNotFound)
                return;

            throw new EntryAlreadyIndexedException(findResult.Model!, findResult.IndexName);
        }
    }
}
