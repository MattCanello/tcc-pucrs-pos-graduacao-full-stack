using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Services;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    static class MockedFeedServiceBuilder
    {
        public static IFeedService CreateInstance(IFeedRepository? feedRepository = null) 
            => new FeedService(feedRepository ?? new MockedFeedRepository());
    }
}
