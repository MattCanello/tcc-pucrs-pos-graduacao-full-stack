using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Domain.Commands;
using MattCanello.NewsFeed.Frontend.Server.Domain.Factories;
using MattCanello.NewsFeed.Frontend.Server.Domain.Handlers;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Profiles;
using MattCanello.NewsFeed.Frontend.Server.Tests.Mocks;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Domain.Handlers
{
    public class NewEntryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_GivenNullCommand_ShouldThrowException()
        {
            var handler = new NewEntryHandler(null!, null!, null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.HandleAsync(null!));

            Assert.Equal("command", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task HandleAsync_GivenValidComamnd_ShouldReportEvent(NewEntryFoundCommand command, SearchDocument searchDocument, Feed feed, Channel channel)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<SearchProfile>(); config.AddProfile<AdminProfile>(); }).CreateMapper();

            var publisher = new MockedNewArticlePublisher();

            var handler = new NewEntryHandler(
                new MockedSearchClient((feedId, id) => searchDocument),
                new ArticleFactory(mapper, new ArticleDetailsFactory()),
                new MockedFeedRepository((feedId) => (feed, channel)),
                publisher);

            await handler.HandleAsync(command);

            var publishedArticle = Assert.Single(publisher.PublishedArticles);

            Assert.Equal(searchDocument.Id, publishedArticle.Id);
            Assert.Equal(feed, publishedArticle.Feed);
            Assert.Equal(channel, publishedArticle.Channel);
        }


        [Theory, AutoData]
        public async Task HandleAsync_GivenNotFoundDocument_ShouldNotReportEvent(NewEntryFoundCommand command, Feed feed, Channel channel)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<SearchProfile>(); config.AddProfile<AdminProfile>(); }).CreateMapper();

            var publisher = new MockedNewArticlePublisher();

            var handler = new NewEntryHandler(
                new MockedSearchClient((string feedId, string id) => null),
                new ArticleFactory(mapper, new ArticleDetailsFactory()),
                new MockedFeedRepository((feedId) => (feed, channel)),
                publisher);

            await handler.HandleAsync(command);

            Assert.Empty(publisher.PublishedArticles);
        }
    }
}
