using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Policies;
using MattCanello.NewsFeed.SearchApi.Tests.Mocks;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Policies
{
    public class PreventDuplicateEntryIndexingPolicyTests
    {
        [Theory, AutoData]
        public async Task EvaluateAsync_GivenANewEntry_ShouldNotThrowExcpetion(IndexEntryCommand indexCommand)
        {
            var policy = new PreventDuplicateEntryIndexingPolicy(
                new MockedDocumentSearchRepository());

            await policy.EvaluateAsync(indexCommand);
        }

        [Theory, AutoData]
        public async Task EvaluateAsync_GivenEmptyEntry_ShouldNotThrowExcpetion(string feedId)
        {
            var policy = new PreventDuplicateEntryIndexingPolicy(
                new MockedDocumentSearchRepository());

            await policy.EvaluateAsync(new IndexEntryCommand() { FeedId = feedId });
        }

        [Theory, AutoData]
        public async Task EvaluateAsync_GivenEmptyEntryId_ShouldNotThrowExcpetion(string feedId, Entry entry)
        {
            var policy = new PreventDuplicateEntryIndexingPolicy(
                new MockedDocumentSearchRepository());

            entry.Id = null;
            await policy.EvaluateAsync(new IndexEntryCommand() { FeedId = feedId, Entry = entry });
        }

        [Theory, AutoData]
        public async Task EvaluateAsync_GivenEmptyFeedId_ShouldNotThrowExcpetion(Entry entry)
        {
            var policy = new PreventDuplicateEntryIndexingPolicy(
                new MockedDocumentSearchRepository());

            await policy.EvaluateAsync(new IndexEntryCommand() { Entry = entry });
        }

        [Theory, AutoData]
        public async Task EvaluateAsync_GivenAlreadyExistingEntry_ShouldThrowException(IndexEntryCommand indexCommand)
        {
            var repository = new MockedDocumentSearchRepository();
            var key = repository.Add(indexCommand.Entry!, indexCommand.FeedId!);

            var policy = new PreventDuplicateEntryIndexingPolicy(repository);

            var exception = await Assert.ThrowsAsync<EntryAlreadyIndexedException>(() => policy.EvaluateAsync(indexCommand));

            Assert.Equal(key.IndexName, exception.IndexName);

            Assert.NotNull(exception.Document);
            Assert.NotNull(exception.Document.Entry);
            Assert.NotNull(exception.Document.FeedId);

            Assert.Equal(indexCommand.Entry!.Id, exception.Document.Entry.Id);
            Assert.Equal(indexCommand.FeedId, exception.Document.FeedId);
            Assert.Equal(key.Id, exception.Document.Id);
        }
    }
}
