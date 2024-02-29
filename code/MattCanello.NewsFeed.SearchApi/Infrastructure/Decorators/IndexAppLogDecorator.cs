using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Decorators
{
    public sealed class IndexAppLogDecorator : IIndexApp
    {
        private readonly ILogger _logger;
        private readonly IIndexApp _innerApp;

        public IndexAppLogDecorator(ILogger logger, IIndexApp innerApp)
        {
            _logger = logger;
            _innerApp = innerApp;
        }

        public async Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            using var scope = _logger.BeginScope(command);

            _logger.LogInformation("Started indexing entry '{id}' from feed '{feedId}'", command.Entry?.Id, command.FeedId);

            var documentId = await _innerApp.IndexAsync(command, cancellationToken);

            _logger.LogInformation("Entry '{id}' from feed '{feedId}' indexed with id '{documentId}'", command.Entry?.Id, command.FeedId, documentId);

            return documentId;
        }
    }
}
