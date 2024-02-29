﻿using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces
{
    public interface IElasticSearchRepository
    {
        Task<string> IndexAsync<TElasticModel>(TElasticModel elasticModel, IndexName indexName, CancellationToken cancellationToken = default) where TElasticModel : class, new();
    }
}