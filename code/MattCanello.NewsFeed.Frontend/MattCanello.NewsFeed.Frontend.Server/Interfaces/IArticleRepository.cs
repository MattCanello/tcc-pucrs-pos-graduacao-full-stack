using MattCanello.NewsFeed.Frontend.Server.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Interfaces
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetFrontPageArticlesAsync();
    }
}
