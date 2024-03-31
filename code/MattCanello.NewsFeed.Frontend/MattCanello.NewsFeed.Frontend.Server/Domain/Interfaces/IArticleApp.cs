using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface IArticleApp
    {
        Task<IEnumerable<Article>> GetFrontPageArticlesAsync(CancellationToken cancellationToken = default);
        Task<Article?> GetArticleAsync(string feedId, string articleId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Article>> GetChannelArticlesAsync(string channelId, CancellationToken cancellationToken = default);
    }
}
