using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Application
{
    public sealed class ArticleApp : IArticleApp
    {
        private readonly ISearchClient _searchClient;
        private readonly IFeedRepository _feedRepository;
        private readonly IArticleFactory _articleFactory;
        private readonly IFrontPageConfiguration _frontPageConfiguration;

        public ArticleApp(ISearchClient searchClient, IFeedRepository feedRepository, IArticleFactory articleFactory, IFrontPageConfiguration frontPageConfiguration)
        {
            _searchClient = searchClient;
            _feedRepository = feedRepository;
            _articleFactory = articleFactory;
            _frontPageConfiguration = frontPageConfiguration;
        }

        public async Task<IEnumerable<Article>> GetFrontPageArticlesAsync(CancellationToken cancellationToken = default)
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
    }
}
