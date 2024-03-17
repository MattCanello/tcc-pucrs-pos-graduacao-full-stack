using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;
using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Exceptions
{
    public class EntryAlreadyIndexedExceptionTests
    {
        [Theory, AutoData]
        public void Constructor_GivenIndexName_ShouldPreserveIndexName(string indexName)
        {
            var exception = new EntryAlreadyIndexedException(indexName);

            Assert.Equal(indexName, exception.IndexName);
            Assert.Equal("The provided entry was already indexed previously.", exception.Message);
        }

        [Theory, AutoData]
        public void Constructor_GivenDocument_ShouldPreserveDocument(Document document, string indexName)
        {
            var exception = new EntryAlreadyIndexedException(document, indexName);

            Assert.Equal(indexName, exception.IndexName);
            Assert.Equal(document, exception.Document);
            Assert.Equal("The provided entry was already indexed previously.", exception.Message);
        }
    }
}
