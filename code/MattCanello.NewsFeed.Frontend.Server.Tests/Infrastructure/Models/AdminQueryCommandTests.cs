using AutoFixture.Xunit2;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Infrastructure.Models
{
    public class AdminQueryCommandTests
    {
        [Fact]
        public void Constructor_GivenEmptyOverload_ShouldProduceDefaultInstance()
        {
            var command = new AdminQueryCommand();

            Assert.Null(command.Skip);
            Assert.Null(command.PageSize);
        }

        [Theory, AutoData]
        public void Constructor_GivenPageSizeOnly_ShouldAssumeDefaultSkip(int pageSize)
        {
            var command = new AdminQueryCommand(pageSize);

            Assert.Equal(pageSize, command.PageSize);
            Assert.Equal(0, command.Skip);
        }

        [Theory, AutoData]
        public void Constructor_GivenParams_ShouldPreserveData(int pageSize, int skip)
        {
            var command = new AdminQueryCommand(pageSize, skip);

            Assert.Equal(pageSize, command.PageSize);
            Assert.Equal(skip, command.Skip);
        }
    }
}
