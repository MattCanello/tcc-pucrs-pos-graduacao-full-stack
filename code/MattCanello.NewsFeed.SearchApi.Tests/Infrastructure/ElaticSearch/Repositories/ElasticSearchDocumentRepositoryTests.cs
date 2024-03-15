using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Builders;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Profiles;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Repositories;
using MattCanello.NewsFeed.SearchApi.Tests.Mocks;

namespace MattCanello.NewsFeed.SearchApi.Tests.Infrastructure.ElaticSearch.Repositories
{
    public class ElasticSearchDocumentRepositoryTests
    {
        private readonly IMapper _defaultMapper = new MapperConfiguration((config) => config.AddProfile<ElasticSearchModelProfile>()).CreateMapper();

        [Theory, AutoData]
        public async Task GetByIdAsync_GivenExistingDocument_ShouldReturnTheDocument(Entry entry, string feedId)
        {
            var fakeRepo = new SuccessMockedElasticSearchRepository<Entry>();
            var indexName = new IndexNameBuilder().WithFeedId(feedId).Build()!;
            var id = await fakeRepo.IndexAsync(entry, indexName);

            var repo = new ElasticSearchDocumentRepository(
                fakeRepo,
                new IndexNameBuilder(),
                _defaultMapper
                );

            var result = await repo.GetByIdAsync(feedId, id);

            Assert.NotNull(result);
            Assert.NotNull(result.Entry);
            Assert.NotNull(result.FeedId);
            Assert.NotNull(result.Id);

            Assert.Equal(entry.EntryId, result.Entry.Id);
            Assert.Equal(feedId, result.FeedId);
            Assert.Equal(id, result.Id);
        }

        [Theory, AutoData]
        public async Task GetByIdAsync_GivenUnknownDocument_ShouldThrowException(string feedId, string id)
        {
            var fakeRepo = new FailMockedElasticSearchRepository<Entry>();

            var repo = new ElasticSearchDocumentRepository(
                fakeRepo,
                new IndexNameBuilder(),
                _defaultMapper
                );

            var exception = await Assert.ThrowsAsync<DocumentNotFoundException>(() => repo.GetByIdAsync(feedId, id));

            Assert.Equal(id, exception.DocumentId);
        }
    }
}
