using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Mocks
{
    sealed class MockedNewArticlePublisher : INewArticlePublisher
    {
        private readonly List<Article> _publishedArticles = new List<Article>();

        public IReadOnlyList<Article> PublishedArticles => _publishedArticles;

        public Task ReportNewArticleAsync(Article article, CancellationToken cancellationToken = default)
        {
            _publishedArticles.Add(article);

            return Task.CompletedTask;
        }
    }
}
