using AutoFixture.Xunit2;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Infrastructure.Configuration
{
    public class FrontPageConfigurationTests
    {
        [Fact]
        public void FrontPageNumberOfArticles_GivenEmptyConfiguration_ShouldReturnDefaultValue()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 0)).Build();

            var config = new FrontPageConfiguration(configuration);

            var frontPageNumberOfArticles = config.FrontPageNumberOfArticles();

            Assert.Equal(10, frontPageNumberOfArticles);
        }

        [Theory, AutoData]
        public void FrontPageNumberOfArticles_GivenValidConfiguration_ShouldReturnExpectedValue(int frontPageArticleCount)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 1) { { "FRONTPAGE_ARTICLE_COUNT", frontPageArticleCount.ToString("F0") } }).Build();

            var config = new FrontPageConfiguration(configuration);

            var frontPageNumberOfArticles = config.FrontPageNumberOfArticles();

            Assert.Equal(frontPageArticleCount, frontPageNumberOfArticles);
        }

        [Theory, AutoData]
        public void FrontPageNumberOfArticles_GivenInvalidConfiguration_ShouldReturnExpectedValue(string frontPageArticleCount)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>(capacity: 1) { { "FRONTPAGE_ARTICLE_COUNT", frontPageArticleCount } }).Build();

            var config = new FrontPageConfiguration(configuration);

            var frontPageNumberOfArticles = config.FrontPageNumberOfArticles();

            Assert.Equal(10, frontPageNumberOfArticles);
        }
    }
}
