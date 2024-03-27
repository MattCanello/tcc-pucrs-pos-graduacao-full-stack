using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Services;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Profiles;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    static class MockedFeedServiceBuilder
    {
        public static IFeedService CreateInstance(IFeedRepository? feedRepository = null)
            => new FeedService(
                feedRepository ?? new MockedFeedRepository(),
                new MapperConfiguration(config => config.AddProfile<FeedProfile>()).CreateMapper());
    }
}
