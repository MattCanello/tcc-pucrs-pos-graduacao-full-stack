using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Nest;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Decorators
{
    public sealed class CachedElasticSearchManagementRepository : IElasticSearchManagementRepository
    {
        private readonly IElasticSearchManagementRepository _elasticSearchManagementRepository;
        private readonly IMemoryCache _memoryCache;

        public CachedElasticSearchManagementRepository(IElasticSearchManagementRepository elasticSearchManagementRepository, IMemoryCache memoryCache)
        {
            _elasticSearchManagementRepository = elasticSearchManagementRepository;
            _memoryCache = memoryCache;
        }

        private static string GetKey(string indexName) 
            => $"idx_{indexName}_exists";

        public async Task EnsureIndexExistsAsync(string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> selector, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetKey(indexName);

            if (_memoryCache.TryGetValue(cacheKey, out _))
                return;

            await _elasticSearchManagementRepository.EnsureIndexExistsAsync(indexName, selector, cancellationToken);

            _memoryCache.Set(cacheKey, true);
        }
    }
}
