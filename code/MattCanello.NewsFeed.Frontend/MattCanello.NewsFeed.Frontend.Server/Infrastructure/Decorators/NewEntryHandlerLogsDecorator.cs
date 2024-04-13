using MattCanello.NewsFeed.Frontend.Server.Domain.Commands;
using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Decorators
{
    public sealed class NewEntryHandlerLogsDecorator : INewEntryHandler
    {
        private readonly INewEntryHandler _handler;
        private readonly ILogger _logger;

        public NewEntryHandlerLogsDecorator(INewEntryHandler handler, ILogger<INewEntryHandler> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(NewEntryFoundCommand command, CancellationToken cancellationToken = default)
        {
            using var scope = _logger.BeginScope(command);

            _logger.LogInformation("Starting handling new entry '{documentId}' from feed '{feedId}'", command.DocumentId, command.FeedId);

            await _handler.HandleAsync(command, cancellationToken);

            _logger.LogInformation("Document '{documentId}' from feed '{feedId}' handleded", command.DocumentId, command.FeedId);
        }
    }
}
