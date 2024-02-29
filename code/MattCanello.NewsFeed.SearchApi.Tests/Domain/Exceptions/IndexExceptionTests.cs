using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Exceptions
{
    public sealed class IndexExceptionTests
    {
        const string DefaultExceptionMessage = "Unable to index entry";

        [Theory, AutoData]
        public void Constructor_GivenInnerException_ShouldPersistException(Exception innerException)
        {
            var exception = new IndexException(innerException);

            Assert.Equal(innerException, exception.InnerException);
        }

        [Theory, AutoData]
        public void Constructor_GivenMessage_ShouldPersistMessage(string message)
        {
            var exception = new IndexException(message);

            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void Constructor_GiveNullMessage_ShouldUseDefaultMessage()
        {
            var exception = new IndexException(message: null);

            Assert.Equal(DefaultExceptionMessage, exception.Message);
        }

        [Fact]
        public void Constructor_GiveNoParam_ShouldUseDefaultMessage()
        {
            var exception = new IndexException();

            Assert.Equal(DefaultExceptionMessage, exception.Message);
        }
    }
}
