using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Application;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Tests.Mocks;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Application
{
    public class ChannelAppTests
    {
        [Fact]
        public async Task GetDocumentsAsync_GivenNullParams_ShouldThrowException()
        {
            var feedApp = new ChannelApp(null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => feedApp.GetDocumentsAsync(null!));

            Assert.Equal("command", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task GetDocumentsAsync_GivenValidCommand_ShouldReturnData(string channelId, Entry entry1, Entry entry2)
        {
            var repository = new MockedDocumentSearchRepository(
                (channelId!, entry1),
                (channelId, entry2));

            var channelApp = new ChannelApp(repository);

            var result = await channelApp.GetDocumentsAsync(new GetChannelDocumentsCommand(channelId));

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Contains(result.Results, r => r.Entry == entry1);
            Assert.Contains(result.Results, r => r.Entry == entry2);
            Assert.Equal(2, result.Total);
            Assert.False(result.IsEmpty);
        }

        [Theory, AutoData]
        public async Task GetDocumentsAsync_GivenChannelIdWithNoMatch_ShouldReturnNoData(string channelId, string entriesChannelId, Entry entry1, Entry entry2)
        {
            var repository = new MockedDocumentSearchRepository(
                (entriesChannelId, entry1),
                (entriesChannelId, entry2));

            var feedApp = new ChannelApp(repository);

            var result = await feedApp.GetDocumentsAsync(new GetChannelDocumentsCommand(channelId));

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Empty(result.Results);
            Assert.Equal(0, result.Total);
            Assert.True(result.IsEmpty);
        }

        [Theory, AutoData]
        public async Task GetDocumentsAsync_GivenEmptyChannelId_ShouldReturnNoEntry(string entry1ChannelId, Entry entry1, string entry2ChannelId, Entry entry2)
        {
            var repository = new MockedDocumentSearchRepository(
                (entry1ChannelId, entry1),
                (entry2ChannelId, entry2));

            var feedApp = new ChannelApp(repository);

            var result = await feedApp.GetDocumentsAsync(new GetChannelDocumentsCommand());

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Empty(result.Results);
            Assert.Equal(0, result.Total);
            Assert.True(result.IsEmpty);
        }
    }
}
