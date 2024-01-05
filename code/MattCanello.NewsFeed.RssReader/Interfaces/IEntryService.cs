using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface IEntryService
    {
        Task ProcessEntriesFromRSSAsync(SyndicationFeed syndicationFeed, CancellationToken cancellationToken = default);
    }
}
