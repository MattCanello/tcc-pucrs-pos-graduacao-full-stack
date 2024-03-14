using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Responses
{
    public class SearchResponseTests
    {
        [Fact]
        public void Constructor_GivenNoParams_ShouldProduceEmptyInstance()
        {
            var response = new SearchResponse<object>();

            Assert.Equal(0, response.Total);
            Assert.Null(response.Results);
            Assert.Null(response.Paging);
            Assert.True(response.IsEmpty);
        }

        [Theory, AutoData]
        public void Constructor_GivenParams_ShouldPreserveData(IReadOnlyList<object> results, Paging paging)
        {
            var response = new SearchResponse<object>(results, results.Count, paging);

            Assert.Equal(results, response.Results);
            Assert.Equal(results.Count, response.Total);
            Assert.Equal(paging, response.Paging);
            Assert.False(response.IsEmpty);
        }

        [Fact]
        public void IsEmpty_GivenTotalAsZero_ShouldBeTrue()
        {
            var response = new SearchResponse<object>
            {
                Total = 0
            };

            Assert.True(response.IsEmpty);
        }
        
        [Theory, AutoData]
        public void IsEmpty_GivenTotal_ShouldBeFalse(int total)
        {
            var response = new SearchResponse<object>
            {
                Total = total
            };

            Assert.False(response.IsEmpty);
            Assert.Equal(total, response.Total);
        }

        [Fact]
        public void CreateEmpty_GivenNullPaging_MustBeEmpty()
        {
            var response = SearchResponse<object>.CreateEmpty();

            Assert.True(response.IsEmpty);
        }

        [Fact]
        public void CreateEmpty_GivenNullPaging_ShouldKeepNullPaging()
        {
            var response = SearchResponse<object>.CreateEmpty();

            Assert.Null(response.Paging);
        }
        
        [Theory, AutoData]
        public void CreateEmpty_GivenPaging_MustBeEmpty(Paging paging)
        {
            var response = SearchResponse<object>.CreateEmpty(paging);

            Assert.True(response.IsEmpty);
        }

        [Theory, AutoData]
        public void CreateEmpty_GivenPaging_ShouldPreservePaging(Paging paging)
        {
            var response = SearchResponse<object>.CreateEmpty(paging);

            Assert.Equal(paging, response.Paging);
        }
    }
}
