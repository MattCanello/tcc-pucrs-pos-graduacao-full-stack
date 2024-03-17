using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Commands
{
    public class GetFeedCommandTests
    {
        [Fact]
        public void Constructor_GivenNoParams_ShouldCreateDefaultInstance()
        {
            var command = new GetFeedCommand();

            Assert.Null(command.FeedId);
            Assert.Equal(GetFeedCommand.DefaultSize, command.Size);
            Assert.Equal(0, command.Skip);
        }

        [Theory, AutoData]
        public void Constructor_GivenSpecificParams_ShouldPreserveGivenData(string feedId, int size, int skip)
        {
            var command = new GetFeedCommand(feedId, size, skip);

            Assert.Equal(feedId, command.FeedId);
            Assert.Equal(size, command.Size);
            Assert.Equal(skip, command.Skip);
        }
    }
}
