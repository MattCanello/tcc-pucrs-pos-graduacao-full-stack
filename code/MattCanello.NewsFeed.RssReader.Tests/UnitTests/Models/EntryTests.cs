using AutoFixture.Xunit2;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Domain.Responses;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Models
{
    public class EntryTests
    {
        [Fact]
        public void ApplyParsedContent_GivenNull_ShouldThrowException()
        {
            var entry = new Entry();

            var ex = Assert.Throws<ArgumentNullException>(() => entry.ApplyParsedContent(null!));

            Assert.NotNull(ex);
            Assert.Equal("parsedContent", ex.ParamName);
        }

        [Theory, AutoData]
        public void ApplyParsedContent_GivenEntryEmptyDescription_ShouldSetDescription(Entry entry, ParsedContent parsedContent)
        {
            entry.Description = null;

            entry.ApplyParsedContent(parsedContent);

            Assert.Equal(parsedContent.Description, entry.Description);
        }

        [Theory, AutoData]
        public void ApplyParsedContent_GivenEntryNullThumbnail_ShouldSetAndAddThumb(Entry entry, ParsedContent parsedContent)
        {
            entry.Thumbnails = null!;

            entry.ApplyParsedContent(parsedContent);

            Assert.NotNull(entry.Thumbnails);
            var singleThumb = Assert.Single(entry.Thumbnails);
            Assert.Equal(parsedContent.Thumbnail, singleThumb);
        }

        [Theory, AutoData]
        public void ApplyParsedContent_GivenNullParsedContentThumb_ShouldNotChangeEntryThumbnails(Entry entry, string description)
        {
            var parsedContent = new ParsedContent(description);
            var previousThumbs = entry.Thumbnails.ToArray();

            entry.ApplyParsedContent(parsedContent);

            Assert.Equivalent(entry.Thumbnails, previousThumbs);
        }

        [Theory, AutoData]
        public void ApplyParsedContent_GivenEntryNonEmptyThumbnail_ShouldAddThumb(Entry entry, ParsedContent parsedContent)
        {
            var previousNumberOfThumb = entry.Thumbnails.Count;
            entry.ApplyParsedContent(parsedContent);

            Assert.NotNull(entry.Thumbnails);
            Assert.Equal(previousNumberOfThumb + 1, entry.Thumbnails.Count);
            Assert.Contains(entry.Thumbnails, t => t == parsedContent.Thumbnail);
        }
    }
}
