using AutoFixture.Xunit2;
using FluentAssertions;
using MattCanello.NewsFeed.SearchApi.Domain.Application;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Tests.Mocks;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Application
{
    public class IndexAppTests
    {
        [Fact]
        public async Task IndexAsync_GivenNullCommand_ShouldThrowException()
        {
            var indexApp = new IndexApp(null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => indexApp.IndexAsync(null!));

            Assert.Equal("command", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task IndexAsync_GivenValidCommand_ShouldIndexDocument(IndexEntryCommand command)
        {
            var service = new MockedEntryIndexService();

            var indexApp = new IndexApp(
                new NoEntryIndexPolicy(),
               service);

            var indexResponse = await indexApp.IndexAsync(command);

            indexResponse
                .Should()
                .NotBeNull();

            service.IndexedCommands
                .Should()
                .ContainSingle()
                .Which
                .Key
                .Should()
                .Be(indexResponse);
        }
    }
}
