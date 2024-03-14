using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Responses
{
    public class SearchResponseTests
    {
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
    }
}
