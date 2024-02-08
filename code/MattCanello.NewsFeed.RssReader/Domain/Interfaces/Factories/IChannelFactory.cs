using MattCanello.NewsFeed.RssReader.Domain.Models;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Factories
{
    public interface IChannelFactory
    {
        Channel FromRSS(SyndicationFeed syndicationFeed);
    }
}
