using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Models
{
    public class PagingTests
    {
        public const int DefaultSize = 10;

        [Fact]
        public void Constructor_GivenNoParams_ShouldProduceDefaultInstance()
        {
            var paging = new Paging();

            Assert.Equal(0, paging.Skip);
            Assert.Equal(DefaultSize, paging.Size);
        }

        [Theory, AutoData]
        public void Constructor_GivenSkip_ShouldPreserveInformedData(int skip)
        {
            var paging = new Paging(skip: skip);

            Assert.Equal(skip, paging.Skip);
            Assert.Equal(DefaultSize, paging.Size);
        }

        [Theory, AutoData]
        public void Constructor_GivenSize_ShouldPreserveInformedData(int size)
        {
            var paging = new Paging(size: size);

            Assert.Equal(0, paging.Skip);
            Assert.Equal(size, paging.Size);
        }

        [Fact]
        public void Constructor_GivenNullParams_ShouldProduceDefaultInstance()
        {
            var paging = new Paging(null, null);

            Assert.Equal(0, paging.Skip);
            Assert.Equal(DefaultSize, paging.Size);
        }
    }
}
