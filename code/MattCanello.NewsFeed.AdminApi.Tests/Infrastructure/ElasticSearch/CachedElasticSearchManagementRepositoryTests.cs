using AutoFixture.Xunit2;
using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Decorators;
using MattCanello.NewsFeed.AdminApi.Tests.Mocks;
using Microsoft.Extensions.Caching.Memory;

namespace MattCanello.NewsFeed.AdminApi.Tests.Infrastructure.ElasticSearch
{
    public class CachedElasticSearchManagementRepositoryTests
    {
        [Theory, AutoData]
        public async Task EnsureIndexExistsAsync_GivenNotPresentKey_ShouldCallUnderneathMethod(string indexName)
        {
            var calledInnerRepository = false;

            var repository = new CachedElasticSearchManagementRepository(
                new MockedElasticSearchManagementRepository((idx, selector) => calledInnerRepository = true),
                new MemoryCache(new MemoryCacheOptions()));

            await repository.EnsureIndexExistsAsync(indexName, null!);

            Assert.True(calledInnerRepository);
        }

        [Theory, AutoData]
        public async Task EnsureIndexExistsAsync_GivenPresentKey_ShouldNotCallUnderneathMethod(string indexName)
        {
            var calledInnerRepository = false;

            var cache = new MemoryCache(new MemoryCacheOptions());
            cache.Set(CachedElasticSearchManagementRepository.GetKey(indexName), true);

            var repository = new CachedElasticSearchManagementRepository(
                new MockedElasticSearchManagementRepository((idx, selector) => calledInnerRepository = true),
                cache);

            await repository.EnsureIndexExistsAsync(indexName, null!);

            Assert.False(calledInnerRepository);
        }
    }
}
