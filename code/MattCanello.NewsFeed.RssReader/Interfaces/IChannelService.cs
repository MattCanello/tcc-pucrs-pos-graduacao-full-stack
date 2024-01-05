using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface IChannelService
    {
        Task ProcessChannelUpdateFromRssAsync(string feedId, SyndicationFeed syndicationFeed, CancellationToken cancellationToken = default);
    }
}
