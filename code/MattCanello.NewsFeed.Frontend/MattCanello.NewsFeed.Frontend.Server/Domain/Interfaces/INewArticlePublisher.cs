using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface INewArticlePublisher
    {
        Task ReportNewArticleAsync(Article article, CancellationToken cancellationToken = default);
    }
}
