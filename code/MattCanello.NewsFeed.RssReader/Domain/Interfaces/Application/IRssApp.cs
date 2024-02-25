using MattCanello.NewsFeed.RssReader.Domain.Responses;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Application
{
    public interface IRssApp
    {
        Task<ProcessRssResponse> ProcessFeedAsync(string feedId, CancellationToken cancellationToken = default);
    }
}
