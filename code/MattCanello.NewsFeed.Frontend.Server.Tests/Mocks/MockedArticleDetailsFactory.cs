using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Mocks
{
    sealed class MockedArticleDetailsFactory : IArticleDetailsFactory
    {
        private readonly Func<string?, ArticleDetails?> _function;

        public MockedArticleDetailsFactory(Func<string?, ArticleDetails?> function) 
            => _function = function ?? throw new ArgumentNullException(nameof(function));

        public ArticleDetails? FromDescription(string? description) 
            => _function(description);
    }
}
