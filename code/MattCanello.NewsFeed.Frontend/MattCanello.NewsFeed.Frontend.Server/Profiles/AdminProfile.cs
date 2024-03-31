using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Models;
using MattCanello.NewsFeed.Frontend.Server.Models.Admin;

namespace MattCanello.NewsFeed.Frontend.Server.Profiles
{
    public sealed class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<AdminChannel, Channel>()
                .ForMember(channel => channel.ChannelName, o => o.MapFrom(adminChannel => adminChannel.Name));

            CreateMap<AdminFeed, Feed>()
                .ForMember(feed => feed.FeedName, o => o.MapFrom(adminFeed => adminFeed.Name));

            CreateMap<AdminFeedWithChannel, Feed>()
                .ForMember(feed => feed.FeedName, o => o.MapFrom(adminFeed => adminFeed.Name));
        }
    }
}
