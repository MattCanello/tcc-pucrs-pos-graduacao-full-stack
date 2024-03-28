using AutoFixture.Xunit2;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;

namespace MattCanello.NewsFeed.AdminApi.Tests.Domain.Exceptions
{
    public class ChannelAlreadyExistsExceptionTests
    {
        [Theory, AutoData]
        public void Constructor_GivenChannelId_ShouldPreserveChannelId(string channelId)
        {
            var ex = new ChannelAlreadyExistsException(channelId);

            Assert.Equal(channelId, ex.ChannelId);
            Assert.Equal("The channel already exists", ex.Message);
        }
    }
}
