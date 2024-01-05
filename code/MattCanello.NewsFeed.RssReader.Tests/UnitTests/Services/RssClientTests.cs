using MattCanello.NewsFeed.RssReader.Domain.Messages;
using MattCanello.NewsFeed.RssReader.Infrastructure.Clients;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using MattCanello.NewsFeed.RssReader.Tests.Properties;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Services
{
    public sealed class RssClientTests
    {
        private const string ETag = "W/\"hash963960047320543834b\"";
        private static readonly DateTimeOffset LastModifiedDate = DateTimeOffset.Parse("Mon, 01 Jan 2024 15:28:01 GMT");

        [Fact]
        public async Task ReadAsync_WithoutETagAndLastModified_ShouldReturnExpected()
        {
            using var httpClient = new HttpClient(new MockedRssHandler(Resources.sample_rss_the_guardian_uk, ETag, LastModifiedDate));
            var client = new RssClient(httpClient);

            var request = new ReadRssRequestMessage(new Uri("https://www.theguardian.com/uk/rss"));
            var response = await client.ReadAsync(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Feed);
            Assert.False(response.IsNotModified);

            Assert.Equal(ETag, response.ETag);
            Assert.Equal(LastModifiedDate, response.ResponseDate);
        }

        [Fact]
        public async Task ReadAsync_WithSameETag_ShouldReturnIsNotModifed()
        {
            using var httpClient = new HttpClient(new MockedRssHandler(Resources.sample_rss_the_guardian_uk, ETag, LastModifiedDate));
            var client = new RssClient(httpClient);

            var request = new ReadRssRequestMessage(new Uri("https://www.theguardian.com/uk/rss"), ETag);
            var response = await client.ReadAsync(request);

            Assert.NotNull(response);
            Assert.Null(response.Feed);
            Assert.True(response.IsNotModified);
        }

        [Fact]
        public async Task ReadAsync_WithSameDate_ShouldReturnIsNotModifed()
        {
            using var httpClient = new HttpClient(new MockedRssHandler(Resources.sample_rss_the_guardian_uk, ETag, LastModifiedDate));
            var client = new RssClient(httpClient);

            var request = new ReadRssRequestMessage(new Uri("https://www.theguardian.com/uk/rss"), lastModifiedDate: LastModifiedDate);
            var response = await client.ReadAsync(request);

            Assert.NotNull(response);
            Assert.Null(response.Feed);
            Assert.True(response.IsNotModified);
        }

        [Fact]
        public async Task ReadAsync_WithPastDate_ShouldReturnIsNotModifed()
        {
            using var httpClient = new HttpClient(new MockedRssHandler(Resources.sample_rss_the_guardian_uk, ETag, LastModifiedDate));
            var client = new RssClient(httpClient);

            var request = new ReadRssRequestMessage(new Uri("https://www.theguardian.com/uk/rss"), lastModifiedDate: LastModifiedDate.AddSeconds(-1));
            var response = await client.ReadAsync(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Feed);
            Assert.False(response.IsNotModified);
        }

        [Fact]
        public async Task ReadAsync_WithDifferentETag_ShouldReturnIsNotModifed()
        {
            using var httpClient = new HttpClient(new MockedRssHandler(Resources.sample_rss_the_guardian_uk, ETag, LastModifiedDate));
            var client = new RssClient(httpClient);

            var request = new ReadRssRequestMessage(new Uri("https://www.theguardian.com/uk/rss"), "W/\"cce6342964cfd77749fc9f0d277c45d965f09892\"");
            var response = await client.ReadAsync(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Feed);
            Assert.False(response.IsNotModified);
        }
    }
}
