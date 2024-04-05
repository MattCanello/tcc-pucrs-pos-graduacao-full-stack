using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Application
{
    public sealed class ArticleApp : IArticleApp
    {
        private readonly ISearchClient _searchClient;
        private readonly IFeedRepository _feedRepository;
        private readonly IArticleFactory _articleFactory;
        private readonly IFrontPageConfiguration _frontPageConfiguration;

        public ArticleApp(
            ISearchClient searchClient,
            IFeedRepository feedRepository,
            IArticleFactory articleFactory,
            IFrontPageConfiguration frontPageConfiguration)
        {
            _searchClient = searchClient;
            _feedRepository = feedRepository;
            _articleFactory = articleFactory;
            _frontPageConfiguration = frontPageConfiguration;
        }

        public async Task<IReadOnlyList<Article>> GetFrontPageArticlesAsync(CancellationToken cancellationToken = default)
        {
            var numberOfArticles = _frontPageConfiguration.FrontPageNumberOfArticles();
            var response = await _searchClient.GetRecentAsync(size: numberOfArticles, cancellationToken: cancellationToken);
            var articles = new List<Article>(capacity: response.Results.Count);

            foreach (var document in response.Results)
            {
                var (feed, channel) = await _feedRepository.GetFeedAndChannelAsync(document.FeedId, cancellationToken);

                var article = _articleFactory.FromSearch(document, channel, feed);
                articles.Add(article);
            }

            return articles;
        }

        public async Task<Article?> GetArticleAsync(string feedId, string articleId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(articleId);

            var getDocumentTask = _searchClient.GetDocumentByIdAsync(feedId, articleId, cancellationToken);
            var getFeedAndChannelTask = _feedRepository.GetFeedAndChannelAsync(feedId, cancellationToken);

            await Task.WhenAll(getDocumentTask, getFeedAndChannelTask);

            var document = getDocumentTask.Result;

            if (document is null)
                return null;

            var (feed, channel) = getFeedAndChannelTask.Result;

            var article = _articleFactory.FromSearch(document, channel, feed);

            return article;
        }

        public async Task<IReadOnlyList<Article>> GetChannelArticlesAsync(string channelId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channelId);
            var numberOfArticles = _frontPageConfiguration.FrontPageNumberOfArticles();

            var searchResults = await _searchClient.GetRecentAsync(channelId, numberOfArticles, cancellationToken);
            var articles = new List<Article>(capacity: searchResults.Results.Count);

            foreach (var document in searchResults.Results)
            {
                var (feed, channel) = await _feedRepository.GetFeedAndChannelAsync(document.FeedId, cancellationToken);

                var article = _articleFactory.FromSearch(document, channel, feed);
                articles.Add(article);
            }

            return articles;
        }

        public async Task<IReadOnlyList<Article>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            var searchResults = await _searchClient.SearchAsync(new SearchCommand() { Query = query }, cancellationToken);

            var articles = new List<Article>(capacity: searchResults.Results.Count);

            foreach (var document in searchResults.Results)
            {
                var (feed, channel) = await _feedRepository.GetFeedAndChannelAsync(document.FeedId, cancellationToken);

                var article = _articleFactory.FromSearch(document, channel, feed);
                articles.Add(article);
            }

            return articles;
        }
    }
}
