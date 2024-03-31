using MattCanello.NewsFeed.Frontend.Server.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Interfaces
{
    public interface IArticleApp
    {
        Task<IEnumerable<Article>> GetFrontPageArticlesAsync(CancellationToken cancellationToken = default);
    }
}
