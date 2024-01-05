using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Factories;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using System.ServiceModel.Syndication;

namespace MattCanello.NewsFeed.RssReader.Domain.Factories
{
    public sealed class ChannelFactory : IChannelFactory
    {
        public Channel FromRSS(SyndicationFeed syndicationFeed)
        {
            ArgumentNullException.ThrowIfNull(syndicationFeed);

            Image? image = null;
            if (syndicationFeed.ImageUrl != null)
                image = new Image() { Url = syndicationFeed.ImageUrl?.ToString() };

            return new Channel()
            {
                Copyright = syndicationFeed.Copyright?.Text,
                Description = syndicationFeed.Description?.Text,
                Language = syndicationFeed.Language,
                Name = syndicationFeed.Title?.Text,
                Url = syndicationFeed.Links?.FirstOrDefault()?.Uri?.ToString(),
                Image = image
            };
        }
    }
}
