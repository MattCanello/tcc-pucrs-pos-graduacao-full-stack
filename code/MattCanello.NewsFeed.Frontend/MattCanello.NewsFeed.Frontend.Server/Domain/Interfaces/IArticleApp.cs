using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface IArticleApp
    {
        Task<IEnumerable<Article>> GetFrontPageArticlesAsync(CancellationToken cancellationToken = default);
    }
}
