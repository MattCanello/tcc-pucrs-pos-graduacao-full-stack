using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Policies;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Profiles;
using MattCanello.NewsFeed.SearchApi.Tests.Mocks;

namespace MattCanello.NewsFeed.SearchApi.Tests.Infrastructure.ElaticSearch.Policies
{
    public class PreventDuplicateEntryIndexingPolicyTests
    {
        private readonly IMapper _defaultMapper = new MapperConfiguration((config) => config.AddProfile<ElasticSearchModelProfile>()).CreateMapper();

        [Theory, AutoData]
        public async Task EvaluateAsync_GivenANewEntry_ShouldNotThrowExcpetion(Entry entry, string indexName)
        {
            var policy = new PreventDuplicateEntryIndexingPolicy(
                new SuccessMockedElasticSearchRepository<Entry>(),
                _defaultMapper);

            await policy.EvaluateAsync(entry, indexName);
        }

        [Theory, AutoData]
        public async Task EvaluateAsync_GivenAlreadyExistingEntry_ShouldThrowException(Entry entry, string indexName)
        {
            var repository = new SuccessMockedElasticSearchRepository<Entry>();
            var indexId = await repository.IndexAsync(entry, indexName);

            var policy = new PreventDuplicateEntryIndexingPolicy(
                repository,
                _defaultMapper);

            var exception = await Assert.ThrowsAsync<EntryAlreadyIndexedException>(() => policy.EvaluateAsync(entry, indexName));

            Assert.Equal(indexName, exception.IndexName);

            Assert.NotNull(exception.Document);
            Assert.NotNull(exception.Document.Entry);
            Assert.NotNull(exception.Document.FeedId);

            Assert.Equal(entry.EntryId, exception.Document.Entry.Id);
            Assert.Equal(entry.FeedId, exception.Document.FeedId);
            Assert.Equal(indexId, exception.Document.Id);
        }
    }
}
