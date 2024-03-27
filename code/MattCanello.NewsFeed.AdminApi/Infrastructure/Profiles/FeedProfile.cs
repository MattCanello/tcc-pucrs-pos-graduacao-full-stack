using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Models;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Profiles
{
    public sealed class FeedProfile : Profile
    {
        public FeedProfile()
        {
            CreateMap<CreateFeedCommand, Feed>()
                .ForMember(feed => feed.CreatedAt, o => o.MapFrom(command => DateTimeOffset.UtcNow))
                .ForMember(feed => feed.Channel, o => o.Ignore());

            CreateMap<Feed, FeedElasticModel>()
                .ForMember(elastic => elastic.ChannelId, o => o.MapFrom(feed => feed.Channel.ChannelId));

            CreateMap<FeedElasticModel, Feed>()
                .ForMember(feed => feed.Channel, o => o.Ignore());

            CreateMap<ChannelData, Feed>()
                .ForMember(feed => feed.FeedId, o => o.Ignore())
                .ForMember(feed => feed.Channel, o => o.Ignore())
                .ForMember(feed => feed.Url, o => o.Ignore())
                .ForMember(feed => feed.CreatedAt, o => o.Ignore())
                .ForMember(feed => feed.Copyright, o => o.MapFrom((channelData, feed) => feed.Copyright ?? channelData.Copyright))
                .ForMember(feed => feed.ImageUrl, o => o.MapFrom((channelData, feed) => feed.ImageUrl ?? channelData.ImageUrl))
                .ForMember(feed => feed.Language, o => o.MapFrom((channelData, feed) => feed.Language ?? channelData.Language))
                .ForMember(feed => feed.Name, o => o.MapFrom((channelData, feed) => feed.Name ?? channelData.Name))
                .ReverseMap();
        }
    }
}
