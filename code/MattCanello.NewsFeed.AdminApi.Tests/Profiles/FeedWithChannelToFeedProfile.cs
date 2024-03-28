using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Tests.Profiles
{
    sealed class FeedWithChannelToFeedProfile : Profile
    {
        public FeedWithChannelToFeedProfile() 
            => CreateMap<FeedWithChannel, Feed>();
    }
}
