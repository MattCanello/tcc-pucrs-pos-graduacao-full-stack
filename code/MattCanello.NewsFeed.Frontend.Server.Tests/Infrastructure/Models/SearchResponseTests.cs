using AutoFixture.Xunit2;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Infrastructure.Models
{
    public class SearchResponseTests
    {
        [Fact]
        public void CreateEmpty_GivenNullParams_ShuoldProduceExpectedResult()
        {
            var response = SearchResponse<object>.CreateEmpty(null, null);

            Assert.NotNull(response);
            Assert.Equal(0, response.Total);

            Assert.NotNull(response.Results);
            Assert.Empty(response.Results);

            Assert.NotNull(response.Paging);
            Assert.Null(response.Paging.Size);
            Assert.Null(response.Paging.Skip);
        }

        [Theory, AutoData]
        public void CreateEmpty_GivenPageSizeAndSkip_ShuoldProduceExpectedResult(int pageSize, int skip)
        {
            var response = SearchResponse<object>.CreateEmpty(pageSize, skip);

            Assert.NotNull(response);
            Assert.Equal(0, response.Total);

            Assert.NotNull(response.Results);
            Assert.Empty(response.Results);

            Assert.NotNull(response.Paging);
            Assert.Equal(pageSize, response.Paging.Size);
            Assert.Equal(skip, response.Paging.Skip);
        }

        [Fact]
        public void CreateEmpty_GivenNullCommand_ShouldThrowException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SearchResponse<object>.CreateEmpty(null!));

            Assert.NotNull(exception);
            Assert.Equal("command", exception.ParamName);
        }

        [Theory, AutoData]
        public void CreateEmpty_GivenCommand_ShouldProduceEquivalentRsponse(SearchCommand command)
        {
            var response = SearchResponse<object>.CreateEmpty(command);

            Assert.NotNull(response);
            Assert.Equal(0, response.Total);

            Assert.NotNull(response.Results);
            Assert.Empty(response.Results);

            Assert.NotNull(response.Paging);
            Assert.Equal(command.PageSize, response.Paging.Size);
            Assert.Equal(command.Skip, response.Paging.Skip);
        }
    }
}
