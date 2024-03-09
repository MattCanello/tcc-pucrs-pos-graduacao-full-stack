using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Exceptions;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Exceptions
{
    public class EntryNotFoundExceptionTests
    {
        const string DefaultExceptionMessage = "Entry not found";

        [Theory, AutoData]
        public void Constructor_GivenInnerException_ShouldPersistException(Exception innerException)
        {
            var exception = new EntryNotFoundException(innerException);

            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void Constructor_GiveNoParam_ShouldUseDefaultMessage()
        {
            var exception = new EntryNotFoundException();

            Assert.Equal(DefaultExceptionMessage, exception.Message);
        }

        [Theory, AutoData]
        public void Constructor_GivenId_ShouldPersistData(string id)
        {
            var exception = new EntryNotFoundException(id);

            Assert.Equal(id, exception.EntryId);
        }

        [Theory, AutoData]
        public void Constructor_GivenId_ShouldUseCustomMessage(string id)
        {
            var exception = new EntryNotFoundException(id);

            Assert.Equal($"The entry '{id}' was not found", exception.Message);
        }

        [Theory]
        [InlineData((string?)null)]
        [InlineData("")]
        public void Constructor_GivenEmptyId_ShouldUseDefaultMessage(string id)
        {
            var exception = new EntryNotFoundException(id);

            Assert.Equal(DefaultExceptionMessage, exception.Message);
        }
    }
}
