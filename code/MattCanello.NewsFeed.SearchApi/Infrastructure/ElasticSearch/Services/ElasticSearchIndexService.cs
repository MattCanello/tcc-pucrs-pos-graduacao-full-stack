﻿using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using Nest;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Services
{
    public sealed class ElasticSearchIndexService : IIndexService
    {
        private readonly IElasticClient _elasticClient;
        private readonly IElasticModelFactory _elasticModelFactory;
        private readonly IElasticSearchExceptionFactory _exceptionFactory;

        public ElasticSearchIndexService(IElasticClient elasticClient, IElasticModelFactory elasticModelFactory, IElasticSearchExceptionFactory exceptionFactory)
        {
            _elasticClient = elasticClient;
            _elasticModelFactory = elasticModelFactory;
            _exceptionFactory = exceptionFactory;
        }

        private static string GetIndexName(string feedId)
            => $"entries-{feedId}";

        public async Task<string> IndexAsync(IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var elasticModel = _elasticModelFactory.CreateElasticModel(command);

            var indexName = GetIndexName(command.FeedId!);

            var indexResponse = await _elasticClient.IndexAsync(new IndexRequest<ElasticSearch.Models.Entry>(index: indexName)
            {
                Document = elasticModel
            }, cancellationToken);

            if (indexResponse.IsValid)
                return indexResponse.Id;

            throw _exceptionFactory.CreateExceptionFromResponse(indexResponse);
        }
    }
}
