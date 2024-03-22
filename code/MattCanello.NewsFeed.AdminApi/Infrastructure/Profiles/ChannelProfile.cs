using AutoMapper;
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
        }
    }
}
