﻿using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.Cross.Abstractions;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Builders;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Factories;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Profiles;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Services;
using MattCanello.NewsFeed.SearchApi.Tests.Mocks;

namespace MattCanello.NewsFeed.SearchApi.Tests.Infrastructure.ElaticSearch.Services
{
    public sealed class ElasticSearchEntryIndexServiceTests
    {
        private readonly IElasticModelFactory _defaultElasticModelFactory = new ElasticModelFactory(
            new MapperConfiguration((config) => config.AddProfile<ElasticSearchModelProfile>()).CreateMapper(),
            new SystemDateTimeProvider());

        [Fact]
        public async Task IndexAsync_GivenNullCommand_ShouldThrowException()
        {
            var app = new ElasticSearchEntryIndexService(null!, null!, null!);

            var argumentException = await Assert.ThrowsAsync<ArgumentNullException>(() => app.IndexAsync(null!));

            Assert.Equal("command", argumentException.ParamName);
        }

        [Theory, AutoData]
        public async Task IndexAsync_GivenValidCommand_ShouldPersistData(IndexEntryCommand command)
        {
            var mockedRepository = new SuccessMockedElasticSearchRepository<Entry>();

            var app = new ElasticSearchEntryIndexService(
                mockedRepository,
                _defaultElasticModelFactory,
                new IndexNameBuilder()
                );

            var result = await app.IndexAsync(command);

            Assert.NotNull(result);

            Assert.Equal(1, mockedRepository.Count);

            Assert.Equal(mockedRepository.LastProducedId, result);
        }

        [Theory, AutoData]
        public async Task IndexAsync_GivenFailedState_ShouldThrowException(IndexEntryCommand command)
        {
            var app = new ElasticSearchEntryIndexService(
                new FailMockedElasticSearchRepository<Entry>(),
                _defaultElasticModelFactory,
                new IndexNameBuilder()
                );

            await Assert.ThrowsAsync<IndexException>(() => app.IndexAsync(command));
        }
    }
}
