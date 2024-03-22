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
        }
    }
}
