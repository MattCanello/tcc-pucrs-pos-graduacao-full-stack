using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Models;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Profiles
{
    public sealed class ChannelProfile : Profile
    {
        public ChannelProfile()
        {
            CreateMap<Channel, ChannelElasticModel>()
                .ReverseMap();

            CreateMap<RssData, Channel>()
                .ForMember(channel => channel.ChannelId, o => o.Ignore())
                .ForMember(channel => channel.Copyright, o => o.MapFrom((data, channel) => channel.Copyright ?? data.Copyright))
                .ForMember(channel => channel.CreatedAt, o => o.Ignore())
                .ForMember(channel => channel.ImageUrl, o => o.MapFrom((data, channel) => channel.ImageUrl ?? data.ImageUrl))
                .ForMember(channel => channel.Language, o => o.MapFrom((data, channel) => channel.Language ?? data.Language))
                .ForMember(channel => channel.Name, o => o.MapFrom((data, channel) => channel.Name ?? data.Name))
                .ForMember(channel => channel.Url, o => o.MapFrom((data, channel) => channel.Url ?? data.Url))
                .ReverseMap();
        }
    }
}
