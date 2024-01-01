using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface IChannelService
    {
        Task ProcessChannelUpdateFromRssAsync(SyndicationFeed syndicationFeed, CancellationToken cancellationToken = default);
    }
}
