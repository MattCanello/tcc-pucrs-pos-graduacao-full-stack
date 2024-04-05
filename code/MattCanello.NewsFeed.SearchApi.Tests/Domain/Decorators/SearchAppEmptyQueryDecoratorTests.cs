using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Application;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Decorators;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Tests.Mocks;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Decorators
{
    public class SearchAppEmptyQueryDecoratorTests
    {
        [Fact]
        public async Task SearchAsync_GivenNullCommand_ShouldThrowException()
        {
            var decorator = new SearchAppEmptyQueryDecorator(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => decorator.SearchAsync(null!));

            Assert.NotNull(exception);
            Assert.Equal("searchCommand", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task SearchAsync_GivenEmptyQuery_ShouldGetRecent(string channelId, string feedId, Entry entry)
        {
            var searchCommand = new SearchCommand(feedId: feedId, channelId: channelId);
            var repository = new MockedDocumentSearchRepository((channelId, feedId, entry));
            var decorator = new SearchAppEmptyQueryDecorator(new SearchApp(new MockedDocumentSearchRepository()), repository);

            var searchResult = await decorator.SearchAsync(searchCommand);

            Assert.NotNull(searchResult);
            Assert.Equal(1, searchResult.Total);
            Assert.NotNull(searchResult.Results);
            Assert.NotEmpty(searchResult.Results);

            var singleEntry = Assert.Single(searchResult.Results);
            Assert.Equal(feedId, singleEntry.FeedId);
            Assert.Equal(entry, singleEntry.Entry);
        }

        [Theory, AutoData]
        public async Task SearchAsync_GivenQuery_ShouldSearchDocuments(string query, string channelId, string feedId, Entry entry)
        {
            var searchCommand = new SearchCommand(query, feedId: feedId, channelId: channelId);
            var repository = new MockedDocumentSearchRepository((channelId, feedId, entry));
            var decorator = new SearchAppEmptyQueryDecorator(new SearchApp(repository), new MockedDocumentSearchRepository());

            var searchResult = await decorator.SearchAsync(searchCommand);

            Assert.NotNull(searchResult);
            Assert.Equal(1, searchResult.Total);
            Assert.NotNull(searchResult.Results);
            Assert.NotEmpty(searchResult.Results);

            var singleEntry = Assert.Single(searchResult.Results);
            Assert.Equal(feedId, singleEntry.FeedId);
            Assert.Equal(entry, singleEntry.Entry);
        }
    }
}
