using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Services
{
    public interface IEntryService
    {
        Task<int> ProcessEntriesFromRSSAsync(string feedId, SyndicationFeed syndicationFeed, CancellationToken cancellationToken = default);
    }
}
