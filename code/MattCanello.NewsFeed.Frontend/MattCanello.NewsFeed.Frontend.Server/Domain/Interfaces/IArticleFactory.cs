using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces
{
    public interface IArticleFactory
    {
        Article FromSearch(SearchDocument document, Channel channel, Feed feed);
    }
}
