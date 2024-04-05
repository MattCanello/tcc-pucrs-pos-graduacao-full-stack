using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Application;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Tests.Mocks;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Application
{
    public class SearchAppTests
    {
        [Fact]
        public async Task SearchAsync_GivenNullParams_ShouldThrowException()
        {
            var searchApp = new SearchApp(null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => searchApp.SearchAsync(null!));

            Assert.Equal("searchCommand", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task SearchAsync_GivenEmptyParams_ShouldReturnAllDocuments(Entry entry1, string feedId1, Entry entry2, string feedId2, Entry entry3, string feedId3)
        {
            var repository = new MockedDocumentSearchRepository(
                (feedId1, entry1),
                (feedId2, entry2),
                (feedId3, entry3)
            );

            var searchApp = new SearchApp(repository);

            var command = new SearchCommand();
            var searchResult = await searchApp.SearchAsync(command);

            Assert.Equal(3, searchResult.Total);
            Assert.Equal(command.Paging, searchResult.Paging);

            Assert.Contains(searchResult.Results!, r => r.Entry == entry1);
            Assert.Contains(searchResult.Results!, r => r.Entry == entry2);
            Assert.Contains(searchResult.Results!, r => r.Entry == entry3);
        }

        [Theory, AutoData]
        public async Task SearchAsync_GivenChannelId_ShouldFilterForChannelId(string channelId, Entry entry1, string feedId1, Entry entry2, string feedId2, Entry entry3, string feedId3)
        {
            var repository = new MockedDocumentSearchRepository(
                (channelId, feedId1, entry1),
                (channelId, feedId2, entry2),
                (channelId, feedId3, entry3)
            );

            var searchApp = new SearchApp(repository);

            var command = new SearchCommand(channelId: channelId);
            var searchResult = await searchApp.SearchAsync(command);

            Assert.Equal(3, searchResult.Total);
            Assert.Equal(command.Paging, searchResult.Paging);

            Assert.Contains(searchResult.Results!, r => r.Entry == entry1);
            Assert.Contains(searchResult.Results!, r => r.Entry == entry2);
            Assert.Contains(searchResult.Results!, r => r.Entry == entry3);
        }
    }
}
