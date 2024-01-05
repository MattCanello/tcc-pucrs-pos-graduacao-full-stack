using MattCanello.NewsFeed.RssReader.Domain.Commands;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Application.Interfaces
{
    public interface ICreateFeedHandler
    {
        Task<Feed> CreateFeedAsync(CreateFeedCommand command);
    }
}
