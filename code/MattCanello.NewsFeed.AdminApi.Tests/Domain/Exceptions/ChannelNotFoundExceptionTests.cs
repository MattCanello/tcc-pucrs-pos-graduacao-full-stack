using AutoFixture.Xunit2;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;

namespace MattCanello.NewsFeed.AdminApi.Tests.Domain.Exceptions
{
    public class ChannelNotFoundExceptionTests
    {
        [Theory, AutoData]
        public void Constructor_GivenChannelId_ShouldPreserveChannelId(string channelId)
        {
            var ex = new ChannelNotFoundException(channelId);

            Assert.Equal(channelId, ex.ChannelId);
            Assert.Equal("The requested channel was not found", ex.Message);
        }
    }
}
