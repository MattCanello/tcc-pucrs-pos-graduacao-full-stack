using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Decorators
{
    public sealed class ArticleAppMetricsDecorator : IArticleApp
    {
        private readonly IArticleApp _articleApp;

        public ArticleAppMetricsDecorator(IArticleApp articleApp)
        {
            _articleApp = articleApp;
        }

        public async Task<Article?> GetArticleAsync(string feedId, string articleId, CancellationToken cancellationToken = default)
        {
            var hits = Metrics.ArticleDetailsHits.CreateCounter<int>("article-details-hits", "Hits", "Article details hits");

            using var activity = ActivitySources.ArticleApp.StartActivity("GetArticleActivity");

            var response = await _articleApp.GetArticleAsync(feedId, articleId, cancellationToken);

            hits.Add(1);

            activity?.SetTag("feedId", feedId);
            activity?.SetTag("articleId", articleId);

            return response;
        }

        public async Task<IReadOnlyList<Article>> GetChannelArticlesAsync(string channelId, CancellationToken cancellationToken = default)
        {
            var hits = Metrics.ChannelHits.CreateCounter<int>("channel-hits", "Hits", "Channel hits");

            using var activity = ActivitySources.ArticleApp.StartActivity("GetChannelArticlesActivity");

            var response = await _articleApp.GetChannelArticlesAsync(channelId, cancellationToken);

            hits.Add(1);

            activity?.SetTag("channelId", channelId);

            return response;
        }

        public async Task<IReadOnlyList<Article>> GetFrontPageArticlesAsync(CancellationToken cancellationToken = default)
        {
            var hits = Metrics.FrontPageHits.CreateCounter<int>("front-page-hits", "Hits", "Front page hits");

            using var activity = ActivitySources.ArticleApp.StartActivity("GetFrontPageArticlesActivity");

            var response = await _articleApp.GetFrontPageArticlesAsync(cancellationToken);

            hits.Add(1);

            return response;
        }

        public async Task<IReadOnlyList<Article>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            var hits = Metrics.SearchHits.CreateCounter<int>("search-hits", "Hits", "Search hits");

            using var activity = ActivitySources.ArticleApp.StartActivity("SearchActivity");

            var response = await _articleApp.SearchAsync(query, cancellationToken);

            hits.Add(1);

            activity?.SetTag("query", query);

            return response;
        }
    }
}
