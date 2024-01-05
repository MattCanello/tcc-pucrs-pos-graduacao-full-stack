using MattCanello.NewsFeed.RssReader.Domain.Models;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Factories
{
    public interface IEntryFactory
    {
        IEnumerable<Entry> FromRSS(SyndicationFeed syndicationFeed);
    }
}
