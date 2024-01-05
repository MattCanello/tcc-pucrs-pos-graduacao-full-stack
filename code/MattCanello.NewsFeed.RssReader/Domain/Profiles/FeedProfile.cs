using AutoMapper;
using MattCanello.NewsFeed.RssReader.Domain.Commands;
using MattCanello.NewsFeed.RssReader.Domain.Messages;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Profiles
{
    public class FeedProfile : Profile
    {
        public FeedProfile()
        {
            CreateMap<CreateFeedCommand, Feed>();

            CreateMap<Feed, ReadRssRequestMessage>()
                .ForMember(message => message.ETag, o => o.MapFrom(feed => feed.LastETag))
                .ForMember(message => message.Uri, o => o.MapFrom(feed => new Uri(feed.Url, UriKind.Absolute)));
        }
    }
}
