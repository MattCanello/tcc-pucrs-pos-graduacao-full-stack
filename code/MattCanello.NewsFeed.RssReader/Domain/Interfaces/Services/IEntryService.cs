using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Domain.Responses;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Services
{
    public interface IEntryService
    {
        Task<PublishRssEntriesResponse> ProcessEntriesFromRSSAsync(Feed feed, SyndicationFeed syndicationFeed, CancellationToken cancellationToken = default);
    }
}
