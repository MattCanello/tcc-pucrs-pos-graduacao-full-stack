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

            CreateMap<RssChannel, Channel>()
                .ForMember(channel => channel.ChannelId, o => o.Ignore())
                .ForMember(channel => channel.Copyright, o => o.MapFrom((data, channel) => channel.Copyright ?? data.Copyright))
                .ForMember(channel => channel.CreatedAt, o => o.Ignore())
                .ForMember(channel => channel.ImageUrl, o => o.MapFrom((data, channel) => channel.ImageUrl ?? data.ImageUrl))
                .ForMember(channel => channel.Name, o => o.MapFrom((data, channel) => channel.Name ?? data.Name))
                .ForMember(channel => channel.Url, o => o.MapFrom((data, channel) => channel.Url ?? data.Url))
                .ReverseMap();

            CreateMap<CreateChannelCommand, Channel>()
                .ForMember(channel => channel.CreatedAt, o => o.Ignore())
                .ForMember(channel => channel.ChannelId, o => o.MapFrom(command => command.ChannelId))
                .ForMember(channel => channel.Copyright, o => o.MapFrom(command => command.Data != null ? command.Data.Copyright : null))
                .ForMember(channel => channel.ImageUrl, o => o.MapFrom(command => command.Data != null ? command.Data.ImageUrl : null))
                .ForMember(channel => channel.Name, o => o.MapFrom(command => command.Data != null ? command.Data.Name : null))
                .ForMember(channel => channel.Url, o => o.MapFrom(command => command.Data != null ? command.Data.Url : null));

            CreateMap<UpdateChannelCommand, Channel>()
                .ForMember(channel => channel.ChannelId, o => o.MapFrom(command => command.ChannelId))
                .ForMember(channel => channel.CreatedAt, o => o.Ignore())
                .ForMember(channel => channel.Copyright, o => o.MapFrom(command => command.Data != null ? command.Data.Copyright : null))
                .ForMember(channel => channel.ImageUrl, o => o.MapFrom(command => command.Data != null ? command.Data.ImageUrl : null))
                .ForMember(channel => channel.Name, o => o.MapFrom(command => command.Data != null ? command.Data.Name : null))
                .ForMember(channel => channel.Url, o => o.MapFrom(command => command.Data != null ? command.Data.Url : null));

            CreateMap<ChannelData, Channel>()
                .ForMember(channel => channel.ChannelId, o => o.Ignore())
                .ForMember(channel => channel.CreatedAt, o => o.Ignore());
        }
    }
}
