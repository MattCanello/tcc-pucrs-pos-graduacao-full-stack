using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Services
{
    public interface IChannelService
    {
        Task ProcessFeedConsumedAsync(string feedId, DateTimeOffset consumedDate, SyndicationFeed syndicationFeed, CancellationToken cancellationToken = default);
    }
}
