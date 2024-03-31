using MattCanello.NewsFeed.Frontend.Server.Models;
using MattCanello.NewsFeed.Frontend.Server.Models.Search;

namespace MattCanello.NewsFeed.Frontend.Server.Interfaces
{
    public interface IArticleFactory
    {
        Article FromSearch(SearchDocument document, Channel channel, Feed feed);
    }
}
