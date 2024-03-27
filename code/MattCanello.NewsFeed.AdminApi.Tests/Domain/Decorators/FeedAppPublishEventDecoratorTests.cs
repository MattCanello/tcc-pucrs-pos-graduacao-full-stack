using AutoFixture.Xunit2;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Decorators;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Tests.Mocks;

namespace MattCanello.NewsFeed.AdminApi.Tests.Domain.Decorators
{
    public class FeedAppPublishEventDecoratorTests
    {
        [Fact]
        public async Task CreateFeedAsync_GivenNullCommand_ShouldThrowException()
        {
            var decorator = new FeedAppPublishEventDecorator(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => decorator.CreateFeedAsync(null!));

            Assert.NotNull(exception);
            Assert.Equal("createFeedCommand", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task CreateFeedAsync_GivenValidCommand_ShouldPublishEvent(CreateFeedCommand command, Feed feed)
        {
            var hasEventBeenPublished = false;
            var eventPublisher = new MockedEventPublisher((feed) => hasEventBeenPublished = true);

            var decorator = new FeedAppPublishEventDecorator(
                new MockedCreateFeedApp((cmd) => feed, (cmd) => feed),
                new MockedEventPublisher((feed) => hasEventBeenPublished = true));

            var resultingFeed = await decorator.CreateFeedAsync(command);

            Assert.NotNull(resultingFeed);
            Assert.Equal(feed, resultingFeed);
            Assert.True(hasEventBeenPublished);
        }
    }
}
