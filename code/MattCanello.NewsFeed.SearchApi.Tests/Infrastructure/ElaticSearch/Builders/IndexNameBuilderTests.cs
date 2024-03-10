using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Builders;

namespace MattCanello.NewsFeed.SearchApi.Tests.Infrastructure.ElaticSearch.Builders
{
    public sealed class IndexNameBuilderTests
    {
        [Fact]
        public void Build_GivenNoFeedId_ShouldReturnNull()
        {
            var builder = new IndexNameBuilder();

            var indexName = builder
                .Build();

            Assert.Null(indexName);
        }

        [Theory, AutoData]
        public void WithFeedId_GivenFeedId_ShouldUseProvidedFeedId(string feedId)
        {
            var builder = new IndexNameBuilder();

            var indexName = builder
                .WithFeedId(feedId)
                .Build();

            Assert.Equal($"entries-{feedId}", indexName);
        }

        [Fact]
        public void Build_AsAllEntriesIndices_ShouldReturnEntriesWildcard()
        {
            var builder = new IndexNameBuilder();

            var indexName = builder
                .AllEntriesIndices()
                .Build();

            Assert.Equal("entries-*", indexName);
        }
    }
}
