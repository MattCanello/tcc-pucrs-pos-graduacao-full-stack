using MattCanello.NewsFeed.Frontend.Server.Domain.Commands;
using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Decorators
{
    public sealed class NewEntryHandlerMetricsDecorator : INewEntryHandler
    {
        private readonly INewEntryHandler _handler;

        public NewEntryHandlerMetricsDecorator(INewEntryHandler handler)
        {
            _handler = handler;
        }

        public async Task HandleAsync(NewEntryFoundCommand command, CancellationToken cancellationToken = default)
        {
            var hits = Metrics.NewEntryHits.CreateCounter<int>("new-entry-hits", "Hits", "New entry hits");

            using var activity = ActivitySources.NewEntryHandler.StartActivity("NewEntryHandler");
  
            await _handler.HandleAsync(command, cancellationToken);

            hits.Add(1);

            activity?.SetTag("feedId", command?.FeedId);
            activity?.SetTag("entryId", command?.EntryId);
            activity?.SetTag("documentId", command?.DocumentId);
        }
    }
}
