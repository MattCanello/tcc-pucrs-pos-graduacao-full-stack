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

            CreateMap<ChannelData, Channel>()
                .ForMember(channel => channel.ChannelId, o => o.Ignore())
                .ForMember(channel => channel.Copyright, o => o.MapFrom((channelData, channel) => channel.Copyright ?? channelData.Copyright))
                .ForMember(channel => channel.CreatedAt, o => o.Ignore())
                .ForMember(channel => channel.ImageUrl, o => o.MapFrom((channelData, channel) => channel.ImageUrl ?? channelData.ImageUrl))
                .ForMember(channel => channel.Language, o => o.MapFrom((channelData, channel) => channel.Language ?? channelData.Language))
                .ForMember(channel => channel.Name, o => o.MapFrom((channelData, channel) => channel.Name ?? channelData.Name))
                .ForMember(channel => channel.Url, o => o.MapFrom((channelData, channel) => channel.Url ?? channelData.Url))
                .ReverseMap();
        }
    }
}
