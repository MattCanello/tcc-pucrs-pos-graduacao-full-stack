using AutoFixture.Xunit2;
using AutoMapper;
using MattCanello.NewsFeed.Cross.Abstractions;
using MattCanello.NewsFeed.Cross.Abstractions.Tests.Mocks;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Factories;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Profiles;

namespace MattCanello.NewsFeed.SearchApi.Tests.Infrastructure.ElaticSearch.Factories
{
    public sealed class ElasticModelFactoryTests
    {
        private readonly IMapper _defaultMapper = new MapperConfiguration((config) => config.AddProfile<ElasticSearchModelProfile>())
            .CreateMapper();

        [Fact]
        public void CreateElasticModel_GivenNullCommand_ShouldThrowException()
        {
            var factory = new ElasticModelFactory(_defaultMapper, new SystemDateTimeProvider());

            Assert.Throws<ArgumentNullException>(() => factory.CreateElasticModel(null!));
        }

        [Theory, AutoData]
        public void CreateElasticModel_GivenNullEntryCommand_ShouldThrowException(IndexEntryCommand command)
        {
            var factory = new ElasticModelFactory(_defaultMapper, new SystemDateTimeProvider());

            command.Entry = null;

            Assert.Throws<ArgumentException>(() => factory.CreateElasticModel(command));
        }

        [Theory, AutoData]
        public void CreateElasticModel_GivenValidCommand_MustHaveSameData(IndexEntryCommand command)
        {
            var dateTimeNow = DateTimeOffset.UtcNow;

            var factory = new ElasticModelFactory(_defaultMapper, new MockedDateTimeProvider(dateTimeNow));

            var entry = factory.CreateElasticModel(command);

            Assert.Equal(command.FeedId, entry.FeedId);
            Assert.Equal(command.Entry!.Id, entry.EntryId);
            Assert.Equal(command.Entry.Title, entry.Title);
            Assert.Equal(command.Entry.Url, entry.Url);
            Assert.Equal(command.Entry.Description, entry.Description);
            Assert.Equal(command.Entry.PublishDate, entry.PublishDate);
            Assert.Equal(dateTimeNow, entry.IndexDate);

            Assert.NotNull(entry.Authors);
            Assert.NotNull(entry.Categories);
            Assert.NotNull(entry.Thumbnails);

            Assert.Equal(command.Entry.Authors.Count, command.Entry.Authors.Count);
            Assert.Equal(command.Entry.Categories.Count, command.Entry.Categories.Count);
            Assert.Equal(command.Entry.Thumbnails.Count, command.Entry.Thumbnails.Count);

            foreach (var providedAuthor in command.Entry.Authors)
            {
                var resultAuthor = entry.Authors.Single(a => a.Url == providedAuthor.URL);
                Assert.NotNull(resultAuthor);

                Assert.Equal(providedAuthor.Name, resultAuthor.Name);
                Assert.Equal(providedAuthor.Email, resultAuthor.Email);
                Assert.Equal(providedAuthor.URL, resultAuthor.Url);
            }

            foreach (var providedCategory in command.Entry.Categories)
            {
                Assert.Single(entry.Categories, (cat) => cat.Name == providedCategory.CategoryName);
            }

            foreach (var providedThumb in command.Entry.Thumbnails)
            {
                var resultThumb = entry.Thumbnails.Single(a => a.Url == providedThumb.Url);
                Assert.NotNull(resultThumb);

                Assert.Equal(providedThumb.Url, resultThumb.Url);
                Assert.Equal(providedThumb.Width, resultThumb.Width);
                Assert.Equal(providedThumb.Credit, resultThumb.Credit);
            }
        }
    }
}
