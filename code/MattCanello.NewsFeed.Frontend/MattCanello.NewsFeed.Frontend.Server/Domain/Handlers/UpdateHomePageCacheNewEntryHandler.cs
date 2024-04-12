using MattCanello.NewsFeed.Frontend.Server.Domain.Commands;
using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Handlers
{
    // TODO: testes
    public sealed class UpdateHomePageCacheNewEntryHandler : INewEntryHandler
    {
        private readonly ISearchClient _searchClient;
        private readonly INewEntryHandler _innerHandler;
        private readonly IHomePageCachingService _homePageCacheingService;

        public UpdateHomePageCacheNewEntryHandler(ISearchClient searchClient, INewEntryHandler innerHandler, IHomePageCachingService homePageCacheingService)
        {
            _searchClient = searchClient;
            _innerHandler = innerHandler;
            _homePageCacheingService = homePageCacheingService;
        }

        public async Task HandleAsync(NewEntryFoundCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var document = await _searchClient
                .GetDocumentByIdAsync(command.FeedId!, command.DocumentId!, cancellationToken);

            if (document is null)
                return;

            await _homePageCacheingService
                .SetLastestArticleDateAsync(document.Entry.PublishDate, cancellationToken);

            await _innerHandler.HandleAsync(command, cancellationToken);
        }
    }
}
