using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Application;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Tests.Mocks;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Application
{
    public class FeedAppTests
    {
        [Fact]
        public async Task GetFeedAsync_GivenNullParams_ShouldThrowException()
        {
            var feedApp = new FeedApp(null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => feedApp.GetFeedAsync(null!));

            Assert.Equal("command", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task GetFeedAsync_GivenValidCommand_ShouldReturnData(string feedId, Entry entry1, Entry entry2)
        {
            var repository = new MockedDocumentSearchRepository(
                (feedId!, entry1),
                (feedId, entry2));

            var feedApp = new FeedApp(repository);

            var result = await feedApp.GetFeedAsync(new GetFeedCommand(feedId));

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Contains(result.Results, r => r.Entry == entry1);
            Assert.Contains(result.Results, r => r.Entry == entry2);
            Assert.Equal(2, result.Total);
            Assert.False(result.IsEmpty);
        }

        [Theory, AutoData]
        public async Task GetFeedAsync_GivenFeedIdWithNoMatch_ShouldReturnNoData(string feedId, string entriesFeedId, Entry entry1, Entry entry2)
        {
            var repository = new MockedDocumentSearchRepository(
                (entriesFeedId, entry1),
                (entriesFeedId, entry2));

            var feedApp = new FeedApp(repository);

            var result = await feedApp.GetFeedAsync(new GetFeedCommand(feedId));

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Empty(result.Results);
            Assert.Equal(0, result.Total);
            Assert.True(result.IsEmpty);
        }

        [Theory, AutoData]
        public async Task GetFeedAsync_GivenEmptyFeedId_ShouldReturnEveryEntry(string entry1FeedId, Entry entry1, string entry2FeedId, Entry entry2)
        {
            var repository = new MockedDocumentSearchRepository(
                (entry1FeedId, entry1),
                (entry2FeedId, entry2));

            var feedApp = new FeedApp(repository);

            var result = await feedApp.GetFeedAsync(new GetFeedCommand());

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Contains(result.Results, r => r.Entry == entry1);
            Assert.Contains(result.Results, r => r.Entry == entry2);
            Assert.Equal(2, result.Total);
            Assert.False(result.IsEmpty);
        }
    }
}
