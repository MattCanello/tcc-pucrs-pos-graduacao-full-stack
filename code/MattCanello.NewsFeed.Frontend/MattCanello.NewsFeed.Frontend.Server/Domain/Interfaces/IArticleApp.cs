using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface IArticleApp
    {
        Task<IReadOnlyList<Article>> GetFrontPageArticlesAsync(CancellationToken cancellationToken = default);
        Task<Article?> GetArticleAsync(string feedId, string articleId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Article>> GetChannelArticlesAsync(string channelId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Article>> SearchAsync(string query, CancellationToken cancellationToken = default);
    }
}
