using AutoFixture.Xunit2;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;

namespace MattCanello.NewsFeed.AdminApi.Tests.Domain.Exceptions
{
    public class NotFoundExceptionTests
    {
        [Fact]
        public void Constructor_GivenNoMessage_ShouldUseDefaultMessage()
        {
            var ex = new NotFoundException();

            Assert.Equal("The requested resource was not found", ex.Message);
        }

        [Theory, AutoData]
        public void Constructor_GivenMessage_ShouldUseGivenMessage(string message)
        {
            var ex = new NotFoundException(message);

            Assert.Equal(message, ex.Message);
        }
    }
}
