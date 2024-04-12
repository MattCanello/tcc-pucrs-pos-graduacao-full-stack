using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Decorators
{
    public sealed class ArticleAppLogsDecorator : IArticleApp
    {
        private readonly IArticleApp _articleApp;
        private readonly ILogger _logger;

        public ArticleAppLogsDecorator(IArticleApp articleApp, ILogger<IArticleApp> logger)
        {
            _articleApp = articleApp;
            _logger = logger;
        }

        public async Task<Article?> GetArticleAsync(string feedId, string articleId, CancellationToken cancellationToken = default)
        {
            using var scope = _logger.BeginScope(new { feedId, articleId });

            _logger.LogInformation("Starting requesting article '{articleId}' from feed '{feedId}'", articleId, feedId);

            var article = await _articleApp.GetArticleAsync(feedId, articleId, cancellationToken);

            if (article is null)
                _logger.LogWarning("Article '{articleId}' from feed '{feedId}' was not found", articleId, feedId);
            else
                _logger.LogInformation("Article '{articleId}' from feed '{feedId}' found", articleId, feedId);

            return article;
        }

        public async Task<IReadOnlyList<Article>> GetChannelArticlesAsync(string channelId, CancellationToken cancellationToken = default)
        {
            using var scope = _logger.BeginScope(new { channelId });

            _logger.LogInformation("Starting requesting channel feed '{channelId}'", channelId);

            var articles = await _articleApp.GetChannelArticlesAsync(channelId, cancellationToken);

            _logger.LogInformation("Channel '{channelId}' returned with {count} articles", channelId, articles.Count);

            return articles;
        }

        public async Task<IReadOnlyList<Article>> GetFrontPageArticlesAsync(CancellationToken cancellationToken = default)
        {
            using var scope = _logger.BeginScope(new { });

            _logger.LogInformation("Starting requesting front page articles");

            var articles = await _articleApp.GetFrontPageArticlesAsync(cancellationToken);

            _logger.LogInformation("Front page articles returned {count} articles", articles.Count);

            return articles;
        }

        public async Task<IReadOnlyList<Article>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            using var scope = _logger.BeginScope(new { query });

            _logger.LogInformation("Starting querying articles with query '{query}'", query);

            var articles = await _articleApp.SearchAsync(query, cancellationToken);

            _logger.LogInformation("Search input '{query}' returned {count} articles", query, articles.Count);

            return articles;
        }
    }
}
