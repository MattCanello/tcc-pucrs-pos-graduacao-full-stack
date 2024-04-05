using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface IArticleDetailsFactory
    {
        ArticleDetails? FromDescription(string? description);
    }
}
