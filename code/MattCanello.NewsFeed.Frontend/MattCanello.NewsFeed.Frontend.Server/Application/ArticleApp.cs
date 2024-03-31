using MattCanello.NewsFeed.Frontend.Server.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Application
{
    public sealed class ArticleApp : IArticleApp
    {
        private readonly ISearchClient _searchClient;
        private readonly IFeedRepository _feedRepository;
        private readonly IArticleFactory _articleFactory;

        public ArticleApp(ISearchClient searchClient, IFeedRepository feedRepository, IArticleFactory articleFactory)
        {
            _searchClient = searchClient;
            _feedRepository = feedRepository;
            _articleFactory = articleFactory;
        }

        public async Task<IEnumerable<Article>> GetFrontPageArticlesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _searchClient.GetRecentAsync(size: 10, cancellationToken: cancellationToken);
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
