using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Commands;
using MattCanello.NewsFeed.RssReader.Domain.Handlers;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Handlers
{
    public sealed class CreateFeedHandlerTests
    {
        [Theory, AutoData]
        public async void CreateFeedAsync_WhenDataIsValid_ShouldCreateFeed(CreateFeedCommand command, Uri url)
        {
            command.Url = url.ToString();

            var handler = new CreateFeedHandler(new InMemoryFeedRepository(), Util.Mapper);
            var feed = await handler.CreateFeedAsync(command);

            Assert.NotNull(feed);
            Assert.Equal(command.Url, feed.Url);
            Assert.Equal(command.FeedId, feed.FeedId);
            Assert.Equal(command.ChannelId, feed.ChannelId);
        }
    }
}
