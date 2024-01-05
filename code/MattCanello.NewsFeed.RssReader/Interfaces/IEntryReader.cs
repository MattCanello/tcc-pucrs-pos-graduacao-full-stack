using MattCanello.NewsFeed.RssReader.Models;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface IEntryReader
    {
        IEnumerable<Entry> FromRSS(SyndicationFeed syndicationFeed);
    }
}
