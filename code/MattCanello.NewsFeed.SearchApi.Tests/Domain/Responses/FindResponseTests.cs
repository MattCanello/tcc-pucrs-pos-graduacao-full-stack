using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Responses
{
    public class FindResponseTests
    {
        [Theory, AutoData]
        public void Constructor_GivenParams_ShouldPreserveData(string id, Entry entry, string indexName)
        {
            var response = new FindResponse<Entry>(id, entry, indexName);

            Assert.Equal(id, response.Id);
            Assert.Equal(entry, response.Model);
            Assert.Equal(indexName, response.IndexName);
        }

        [Theory, AutoData]
        public void Constructor_GivenModel_ShouldNotBeNotFound(string id, Entry entry, string indexName)
        {
            var response = new FindResponse<Entry>(id, entry, indexName);

            Assert.False(response.IsNotFound);
        }
    }
}
