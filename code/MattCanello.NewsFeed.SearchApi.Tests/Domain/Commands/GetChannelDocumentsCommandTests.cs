using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Commands
{
    public class GetChannelDocumentsCommandTests
    {
        [Fact]
        public void Constructor_GivenNoParams_ShouldCreateDefaultInstance()
        {
            var command = new GetChannelDocumentsCommand();

            Assert.Empty(command.ChannelId);
            Assert.Equal(GetChannelDocumentsCommand.DefaultSize, command.Size);
            Assert.Equal(0, command.Skip);
        }

        [Theory, AutoData]
        public void Constructor_GivenSpecificParams_ShouldPreserveGivenData(string channelId, int size, int skip)
        {
            var command = new GetChannelDocumentsCommand(channelId, size, skip);

            Assert.Equal(channelId, command.ChannelId);
            Assert.Equal(size, command.Size);
            Assert.Equal(skip, command.Skip);
        }
    }
}
