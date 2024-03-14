using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.SearchApi.Domain.Application
{
    public sealed class IndexApp : IIndexApp
    {
        private readonly IEntryIndexPolicy _entryIndexPolicy;
        private readonly IEntryIndexService _entryIndexService;

        public IndexApp(IEntryIndexPolicy entryIndexPolicy, IEntryIndexService entryIndexService)
        {
            _entryIndexPolicy = entryIndexPolicy;
            _entryIndexService = entryIndexService;
        }

        public async Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            await _entryIndexPolicy.EvaluateAsync(command, cancellationToken);

            var documentId = await _entryIndexService.IndexAsync(command, cancellationToken);

            return documentId;
        }
    }
}
