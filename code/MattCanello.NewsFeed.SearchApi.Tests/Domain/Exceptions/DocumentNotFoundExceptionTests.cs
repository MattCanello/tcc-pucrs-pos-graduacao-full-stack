using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Exceptions
{
    public class DocumentNotFoundExceptionTests
    {
        const string DefaultExceptionMessage = "Document not found";

        [Theory, AutoData]
        public void Constructor_GivenInnerException_ShouldPersistException(Exception innerException)
        {
            var exception = new DocumentNotFoundException(innerException);

            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void Constructor_GiveNoParam_ShouldUseDefaultMessage()
        {
            var exception = new DocumentNotFoundException();

            Assert.Equal(DefaultExceptionMessage, exception.Message);
        }

        [Theory, AutoData]
        public void Constructor_GivenId_ShouldPersistData(string id)
        {
            var exception = new DocumentNotFoundException(id);

            Assert.Equal(id, exception.DocumentId);
        }

        [Theory, AutoData]
        public void Constructor_GivenId_ShouldUseCustomMessage(string id)
        {
            var exception = new DocumentNotFoundException(id);

            Assert.Equal($"The document '{id}' was not found", exception.Message);
        }

        [Theory]
        [InlineData((string?)null)]
        [InlineData("")]
        public void Constructor_GivenEmptyId_ShouldUseDefaultMessage(string id)
        {
            var exception = new DocumentNotFoundException(id);

            Assert.Equal(DefaultExceptionMessage, exception.Message);
        }
    }
}
