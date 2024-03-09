using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Exceptions;

namespace MattCanello.NewsFeed.SearchApi.Tests.Infrastructure.ElaticSearch.Exceptions
{
    public sealed class ElasticSearchExceptionTests
    {
        const string DefaultExceptionMessage = "ElasticSearch operation error";

        [Theory, AutoData]
        public void Constructor_GivenInnerException_ShouldPersistException(Exception innerException)
        {
            var exception = new ElasticSearchException(innerException);

            Assert.Equal(innerException, exception.InnerException);
        }

        [Theory, AutoData]
        public void Constructor_GivenMessage_ShouldPersistMessage(string message)
        {
            var exception = new ElasticSearchException(message, innerException: null);

            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void Constructor_GiveNullMessage_ShouldUseDefaultMessage()
        {
            var exception = new ElasticSearchException(message: null);

            Assert.Equal(DefaultExceptionMessage, exception.Message);
        }

        [Fact]
        public void Constructor_GiveNoParam_ShouldUseDefaultMessage()
        {
            var exception = new ElasticSearchException();

            Assert.Equal(DefaultExceptionMessage, exception.Message);
        }

        [Theory, AutoData]
        public void Constructor_GivenIndexName_ShouldPersistData(string indexName)
        {
            var exception = new ElasticSearchException(indexName, message: null, innerException: null);

            Assert.Equal(indexName, exception.IndexName);
        }
    }
}
