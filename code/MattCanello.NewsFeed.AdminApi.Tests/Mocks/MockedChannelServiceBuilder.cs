using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Services;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Profiles;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    static class MockedChannelServiceBuilder
    {
        public static IChannelService CreateInstance(IChannelRepository? channelRepository = null)
        {
            return new ChannelService(
                channelRepository ?? new MockedChannelRepository(),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());
        }
    }
}
