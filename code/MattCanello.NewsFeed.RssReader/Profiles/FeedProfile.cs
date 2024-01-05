using AutoMapper;
using MattCanello.NewsFeed.RssReader.Events;
using MattCanello.NewsFeed.RssReader.Messages;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Profiles
{
    public class FeedProfile : Profile
    {
        public FeedProfile()
        {
            CreateMap<CreateFeedMessage, Feed>();

            CreateMap<Feed, ReadRssRequestMessage>()
                .ForMember(message => message.ETag, o => o.MapFrom(feed => feed.LastETag))
                .ForMember(message => message.Uri, o => o.MapFrom(feed => new Uri(feed.Url, UriKind.Absolute)));
        }
    }
}
