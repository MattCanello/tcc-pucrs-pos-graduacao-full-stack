using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Domain.Application;
using MattCanello.NewsFeed.Frontend.Server.Domain.Factories;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Profiles;
using MattCanello.NewsFeed.Frontend.Server.Tests.Mocks;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Domain.Application
{
    public class ArticleAppTests
    {
        [Theory, AutoData]
        public async Task GetFrontPageArticlesAsync_GivenValidConfiguation_ShouldReturnData(Feed feed, Channel channel, SearchResponse<SearchDocument> searchResponse)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<AdminProfile>(); config.AddProfile<SearchProfile>(); }).CreateMapper();

            var app = new ArticleApp(
                new MockedSearchClient((channelId, size) => searchResponse),
                new MockedFeedRepository((feedId) => (feed, channel)),
                new ArticleFactory(mapper, new ArticleDetailsFactory()),
                new MockedFrontPageConfiguration(5));

            var frontPageArticles = await app.GetFrontPageArticlesAsync();

            Assert.NotNull(frontPageArticles);
            Assert.NotEmpty(frontPageArticles);

            Assert.Equal(searchResponse.Results.Count, frontPageArticles.Count);
        }

        [Fact]
        public async Task GetArticleAsync_GivenNullFeedId_ShouldThrowException()
        {
            var app = new ArticleApp(null!, null!, null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => app.GetArticleAsync(null!, null!));

            Assert.Equal("feedId", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task GetArticleAsync_GivenNullArticleId_ShouldThrowException(string feedId)
        {
            var app = new ArticleApp(null!, null!, null!, null!);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => app.GetArticleAsync(feedId, null!));

            Assert.Equal("articleId", exception.ParamName);
        }

        [Theory, AutoData]
        public async Task GetArticleAsync_GivenExistingArticle_ShouldReturnArticle(SearchDocument searchDocument, Feed feed, Channel channel)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<AdminProfile>(); config.AddProfile<SearchProfile>(); }).CreateMapper();

            var app = new ArticleApp(
                new MockedSearchClient((feedId, articleId) => searchDocument),
                new MockedFeedRepository((feedId) => (feed, channel)),
                new ArticleFactory(mapper, new ArticleDetailsFactory()),
                new MockedFrontPageConfiguration(5));

            var article = await app.GetArticleAsync(searchDocument.FeedId, searchDocument.Id!);

            Assert.NotNull(article);
            Assert.Equal(searchDocument.Id, article.Id);
            Assert.Equal(feed, article.Feed);
            Assert.Equal(channel, article.Channel);
            Assert.Equal(searchDocument.Entry.Url, article.Url);
        }

        [Theory, AutoData]
        public async Task GetArticleAsync_GivenNotExistingArticle_ShouldReturnArticle(string docId, Feed feed, Channel channel)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<AdminProfile>(); config.AddProfile<SearchProfile>(); }).CreateMapper();

            var app = new ArticleApp(
                new MockedSearchClient((feedId, articleId) => (SearchDocument?)null),
                new MockedFeedRepository((feedId) => (feed, channel)),
                new ArticleFactory(mapper, new ArticleDetailsFactory()),
                new MockedFrontPageConfiguration(5));

            var article = await app.GetArticleAsync(feed.FeedId!, docId);

            Assert.Null(article);
        }

        [Theory, AutoData]
        public async Task GetChannelArticlesAsync_GivenExistingArticle_ShouldReturnArticle(SearchResponse<SearchDocument> searchResponse, Feed feed, Channel channel)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<AdminProfile>(); config.AddProfile<SearchProfile>(); }).CreateMapper();

            var app = new ArticleApp(
                new MockedSearchClient((channelId, size) => searchResponse),
                new MockedFeedRepository((feedId) => (feed, channel)),
                new ArticleFactory(mapper, new ArticleDetailsFactory()),
                new MockedFrontPageConfiguration(5));

            var article = await app.GetChannelArticlesAsync(channel.ChannelId!);

            Assert.NotNull(article);
            Assert.NotEmpty(article);
            Assert.Equal(searchResponse.Results.Count, article.Count);
        }

        [Theory, AutoData]
        public async Task SearchAsync_GivenExistingArticle_ShouldReturnArticle(SearchResponse<SearchDocument> searchResponse, Feed feed, Channel channel)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<AdminProfile>(); config.AddProfile<SearchProfile>(); }).CreateMapper();

            var app = new ArticleApp(
                new MockedSearchClient((cmd) => searchResponse),
                new MockedFeedRepository((feedId) => (feed, channel)),
                new ArticleFactory(mapper, new ArticleDetailsFactory()),
                new MockedFrontPageConfiguration(5));

            var article = await app.SearchAsync(channel.ChannelId!);

            Assert.NotNull(article);
            Assert.NotEmpty(article);
            Assert.Equal(searchResponse.Results.Count, article.Count);
        }
    }
}
