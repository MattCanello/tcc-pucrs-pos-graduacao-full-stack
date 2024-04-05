using AutoFixture.Xunit2;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Infrastructure.Models
{
    public class SearchPagingTests
    {
        [Fact]
        public void Constructor_GivenEmptyOverload_ShouldProduceDefaultInstance()
        {
            var paging = new SearchPaging();

            Assert.Null(paging.Skip);
            Assert.Null(paging.Size);
        }

        [Fact]
        public void Constructor_GivenNullParams_ShouldProduceDefaultInstance()
        {
            var paging = new SearchPaging(null, null);

            Assert.Null(paging.Skip);
            Assert.Null(paging.Size);
        }

        [Theory, AutoData]
        public void Constructor_GivenParams_ShouldPreserveData(int pageSize, int skip)
        {
            var paging = new SearchPaging(pageSize, skip);

            Assert.Equal(pageSize, paging.Size);
            Assert.Equal(skip, paging.Skip);
        }
    }
}
