using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.EventPublishers
{
    public sealed class NewArticlePublisher : INewArticlePublisher
    {
        private readonly IHubContext<ArticleHub> _hub;

        public NewArticlePublisher(IHubContext<ArticleHub> hub)
            => _hub = hub;

        public async Task ReportNewArticleAsync(Article article, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(article);

            await _hub.Clients.All.SendAsync("NewArticleFound", article, cancellationToken);
        }
    }
}
