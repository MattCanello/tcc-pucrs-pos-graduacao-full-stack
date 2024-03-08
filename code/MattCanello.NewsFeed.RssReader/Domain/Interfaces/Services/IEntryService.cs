using MattCanello.NewsFeed.RssReader.Domain.Responses;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Services
{
    public interface IEntryService
    {
        Task<PublishRssEntriesResponse> ProcessEntriesFromRSSAsync(string feedId, SyndicationFeed syndicationFeed, DateTimeOffset? lastPublishedEntryDate, CancellationToken cancellationToken = default);
    }
}
