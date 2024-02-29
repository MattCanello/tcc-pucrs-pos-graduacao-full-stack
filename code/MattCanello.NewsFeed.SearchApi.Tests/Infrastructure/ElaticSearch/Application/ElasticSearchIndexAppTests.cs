using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Builders;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Factories;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Profiles;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Services;
using MattCanello.NewsFeed.SearchApi.Tests.Mocks;

namespace MattCanello.NewsFeed.SearchApi.Tests.Infrastructure.ElaticSearch.Application
{
    public sealed class ElasticSearchIndexAppTests
    {
        private readonly IMapper _defaultMapper = new MapperConfiguration((config) => config.AddProfile<ElasticSearchModelProfile>())
            .CreateMapper();

        [Fact]
        public async Task IndexAsync_GivenNullCommand_ShouldThrowException()
        {
            var app = new ElasticSearchIndexApp(null!, null!, null!);

            var argumentException = await Assert.ThrowsAsync<ArgumentNullException>(() => app.IndexAsync(null!));

            Assert.Equal("command", argumentException.ParamName);
        }

        [Theory, AutoData]
        public async Task IndexAsync_GivenValidCommand_ShouldPersistData(IndexEntryCommand command)
        {
            var mockedRepository = new SuccessMockedElasticSearchRepository();

            var app = new ElasticSearchIndexApp(
                mockedRepository,
                new ElasticModelFactory(_defaultMapper),
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
            var app = new ElasticSearchIndexApp(
                new FailMockedElasticSearchRepository(),
                new ElasticModelFactory(_defaultMapper),
                new IndexNameBuilder()
                );

            await Assert.ThrowsAsync<IndexException>(() => app.IndexAsync(command));
        }
    }
}
