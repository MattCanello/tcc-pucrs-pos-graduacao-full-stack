using MattCanello.NewsFeed.RssReader.Domain.Commands;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Handlers
{
    public interface ICreateFeedHandler
    {
        Task<Feed> CreateFeedAsync(CreateFeedCommand command);
    }
}
