using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Domain.Factories;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Profiles;
using MattCanello.NewsFeed.Frontend.Server.Tests.Mocks;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Domain.Factories
{
    public class ArticleFactoryTests
    {
        [Fact]
        public void FromSearch_GivenNullDocument_ShouldThrowException()
        {
            var factory = new ArticleFactory(null!, null!);

            var exception = Assert.Throws<ArgumentNullException>(() => factory.FromSearch(null!, null!, null!));

            Assert.Equal("document", exception.ParamName);
        }

        [Fact]
        public void CreateAuthors_GivenNull_ShouldThrowException()
        {
            var factory = new ArticleFactory(null!, null!);

            var exception = Assert.Throws<ArgumentNullException>(() => factory.CreateAuthors(null!));

            Assert.Equal("authors", exception.ParamName);
        }

        [Fact]
        public void SelectThumbnail_GivenNull_ShouldThrowException()
        {
            var factory = new ArticleFactory(null!, null!);

            var exception = Assert.Throws<ArgumentNullException>(() => factory.SelectThumbnail(null!));

            Assert.Equal("searchThumbnails", exception.ParamName);
        }

        [Theory, AutoData]
        public void FromSearch_GivenValidData_ShouldProduceExpectedResult(SearchDocument document, Channel channel, Feed feed, ArticleDetails articleDetails)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<AdminProfile>(); config.AddProfile<SearchProfile>(); }).CreateMapper();

            var factory = new ArticleFactory(mapper, new MockedArticleDetailsFactory((desc) => articleDetails));

            var article = factory.FromSearch(document, channel, feed);

            Assert.Equal(document.Id, article.Id);
            Assert.Equal(channel, article.Channel);
            Assert.Equal(feed, article.Feed);
            Assert.Equal(document.Entry.PublishDate, article.PublishDate);
            Assert.Equal(document.Entry.Title, article.Title);
            Assert.Equal(articleDetails, article.Details);
            Assert.Equal(document.Entry.Url, article.Url);
        }

        [Theory, AutoData]
        public void CreateAuthors_GivenSingleAuthor_ShouldReturnSingleAuthor(SearchAuthor searchAuthor)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<AdminProfile>(); config.AddProfile<SearchProfile>(); }).CreateMapper();

            var factory = new ArticleFactory(mapper, null!);

            var authors = factory.CreateAuthors(new List<SearchAuthor>(capacity: 1) { searchAuthor });

            Assert.NotNull(authors);

            var singleAuthor = Assert.Single(authors);
            Assert.Equal(searchAuthor.Name, singleAuthor.Name);
            Assert.Equal(searchAuthor.Email, singleAuthor.Email);
        }

        [Theory, AutoData]
        public void CreateAuthors_GivenSameAuthorTwice_ShouldReturnSingleAuthor(SearchAuthor searchAuthor)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<AdminProfile>(); config.AddProfile<SearchProfile>(); }).CreateMapper();

            var factory = new ArticleFactory(mapper, null!);

            var clonedAuthor = new SearchAuthor() { Email = searchAuthor.Email, Name = searchAuthor.Name, URL = searchAuthor.URL };
            var authors = factory.CreateAuthors(new List<SearchAuthor>(capacity: 2) { searchAuthor, clonedAuthor });

            Assert.NotNull(authors);

            var singleAuthor = Assert.Single(authors);
            Assert.Equal(searchAuthor.Name, singleAuthor.Name);
            Assert.Equal(searchAuthor.Email, singleAuthor.Email);
        }

        [Theory, AutoData]
        public void CreateAuthors_GivenTwoDifferentAuthors_ShouldReturnTwoAuthor(SearchAuthor searchAuthor1, SearchAuthor searchAuthor2)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<AdminProfile>(); config.AddProfile<SearchProfile>(); }).CreateMapper();

            var factory = new ArticleFactory(mapper, null!);

            var authors = factory.CreateAuthors(new List<SearchAuthor>(capacity: 2) { searchAuthor1, searchAuthor2 });

            Assert.NotNull(authors);

            Assert.Contains(authors, (a) =>
                (a.Name == searchAuthor1.Name && a.Email == searchAuthor1.Email)
                || (a.Name == searchAuthor2.Name && a.Email == searchAuthor2.Email));
        }

        [Theory, AutoData]
        public void SelectThumbnail_GivenSingleThumb_ShouldReturnEquivalentThumb(SearchThumbnail searchThumb)
        {
            var mapper = new MapperConfiguration(config => { config.AddProfile<AdminProfile>(); config.AddProfile<SearchProfile>(); }).CreateMapper();

            var factory = new ArticleFactory(mapper, null!);

            var selectedThumb = factory.SelectThumbnail(new List<SearchThumbnail>(capacity: 1) { searchThumb });

            Assert.NotNull(selectedThumb);
            Assert.Equal(searchThumb.Credit, selectedThumb.Credit);
            Assert.Equal(searchThumb.Url, selectedThumb.ImageUrl);
            Assert.Equal(searchThumb.Width, selectedThumb.Width);
            Assert.Null(selectedThumb.Caption);
        }

        [Theory, AutoData]
        public void SelectThumbnail_GivenTwoThumbs_ShouldSelectLargerThumb(SearchThumbnail searchThumb1, SearchThumbnail searchThumb2)
        {
            var largerThumb = searchThumb1.Width >= searchThumb2.Width ? searchThumb1 : searchThumb2;

            var mapper = new MapperConfiguration(config => { config.AddProfile<AdminProfile>(); config.AddProfile<SearchProfile>(); }).CreateMapper();

            var factory = new ArticleFactory(mapper, null!);

            var selectedThumb = factory.SelectThumbnail(new List<SearchThumbnail>(capacity: 2) { searchThumb1, searchThumb2 });

            Assert.NotNull(selectedThumb);
            Assert.Equal(largerThumb.Credit, selectedThumb.Credit);
            Assert.Equal(largerThumb.Url, selectedThumb.ImageUrl);
            Assert.Equal(largerThumb.Width, selectedThumb.Width);
        }
    }
}
