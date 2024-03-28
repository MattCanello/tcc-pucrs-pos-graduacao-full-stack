using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Services;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Profiles;
using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;
using MattCanello.NewsFeed.Cross.Abstractions.Tests.Mocks;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    static class MockedChannelServiceBuilder
    {
        public static IChannelService CreateInstance(IChannelRepository? channelRepository = null, IDateTimeProvider? dateTimeProvider = null)
        {
            return new ChannelService(
                channelRepository ?? new MockedChannelRepository(),
                dateTimeProvider ?? new MockedDateTimeProvider(),
                new MapperConfiguration(config => config.AddProfile<ChannelProfile>()).CreateMapper());
        }
    }
}
