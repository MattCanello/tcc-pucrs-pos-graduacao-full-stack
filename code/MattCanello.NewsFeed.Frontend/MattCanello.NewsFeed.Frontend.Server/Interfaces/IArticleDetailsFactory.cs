using MattCanello.NewsFeed.Frontend.Server.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Interfaces
{
    public interface IArticleDetailsFactory
    {
        ArticleDetails? FromDescription(string? description);
    }
}
