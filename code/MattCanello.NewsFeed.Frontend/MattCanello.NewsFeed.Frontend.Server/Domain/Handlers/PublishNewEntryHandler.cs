using MattCanello.NewsFeed.Frontend.Server.Domain.Commands;
using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Handlers
{
    public sealed class PublishNewEntryHandler : INewEntryHandler
    {
        private readonly ISearchClient _searchClient;
        private readonly IArticleFactory _articleFactory;
        private readonly IFeedRepository _feedRepository;
        private readonly INewArticlePublisher _newArticlePublisher;

        public PublishNewEntryHandler(ISearchClient searchClient, IArticleFactory articleFactory, IFeedRepository feedRepository, INewArticlePublisher newArticlePublisher)
        {
            _searchClient = searchClient;
            _articleFactory = articleFactory;
            _feedRepository = feedRepository;
            _newArticlePublisher = newArticlePublisher;
        }

        public async Task HandleAsync(NewEntryFoundCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var getDocumentTask = _searchClient.GetDocumentByIdAsync(command.FeedId!, command.DocumentId!, cancellationToken);
            var getFeedTask = _feedRepository.GetFeedAndChannelAsync(command.FeedId!, cancellationToken);

            await Task.WhenAll(getDocumentTask, getFeedTask);

            var document = getDocumentTask.Result;

            if (document is null)
                return;

            var (feed, channel) = getFeedTask.Result;

            var article = _articleFactory.FromSearch(document, channel, feed);
            
            await _newArticlePublisher.ReportNewArticleAsync(article, cancellationToken);
        }
    }
}
