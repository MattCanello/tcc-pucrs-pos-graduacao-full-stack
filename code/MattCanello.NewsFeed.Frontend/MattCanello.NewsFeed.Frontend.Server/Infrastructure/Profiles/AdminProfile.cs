using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Profiles
{
    public sealed class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<AdminChannel, Channel>();

            CreateMap<AdminFeed, Feed>();

            CreateMap<AdminFeedWithChannel, Feed>();
        }
    }
}
