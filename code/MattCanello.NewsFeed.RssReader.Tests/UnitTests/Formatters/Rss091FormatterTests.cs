using MattCanello.NewsFeed.RssReader.Infrastructure.Formatters.Rss091;
using MattCanello.NewsFeed.RssReader.Tests.Properties;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Tests.UnitTests.Formatters
{
    public sealed class Rss091FormatterTests
    {
        private static XmlReaderSettings ReaderSettings => new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore };

        [Fact]
        public void CanRead_Rss091Sample_ShouldReturnTrue()
        {
            var reader = XmlReader.Create(new StringReader(Resources.sample_rss_091), ReaderSettings);

            var formatter = new Rss091Formatter();

            var canRead = formatter.CanRead(reader);

            Assert.True(canRead);
        }

        [Fact]
        public void CanRead_TheGuardianSample_ShouldReturnFalse()
        {
            var reader = XmlReader.Create(new StringReader(Resources.sample_rss_the_guardian_uk), ReaderSettings);

            var formatter = new Rss091Formatter();

            var canRead = formatter.CanRead(reader);

            Assert.False(canRead);
        }

        [Fact]
        public void ReadFrom_Rss091Sample_ShouldProduceExpectedResult()
        {
            var reader = XmlReader.Create(new StringReader(Resources.sample_rss_091), ReaderSettings);

            var formatter = new Rss091Formatter();
            formatter.ReadFrom(reader);

            var feed = formatter.Feed;
            Assert.NotNull(feed);

            Assert.NotNull(feed.Title);
            Assert.Equal("Scripting News", feed.Title.Text);

            Assert.NotNull(feed.Copyright);
            Assert.Equal("Copyright 1997-1999 UserLand Software, Inc.", feed.Copyright.Text);

            Assert.Equal(DateTimeOffset.Parse("Thu, 08 Jul 1999 07:00:00 GMT"), feed.LastUpdatedTime);

            Assert.NotNull(feed.Documentation);
            Assert.NotNull(feed.Documentation.Uri);
            Assert.Equal("http://my.userland.com/stories/storyReader$11", feed.Documentation.Uri.ToString());

            Assert.NotNull(feed.Description);
            Assert.Equal("News and commentary from the cross-platform scripting community.", feed.Description.Text);

            Assert.Contains(feed.Links, link => link.Uri.ToString() == "http://www.scripting.com/");

            Assert.NotNull(feed.ImageUrl);
            Assert.Equal("http://www.scripting.com/gifs/tinyScriptingNews.gif", feed.ImageUrl.ToString());

            Assert.NotEmpty(feed.Authors);
            var singleAuthor = Assert.Single(feed.Authors);

            Assert.Equal("Dave Winer", singleAuthor.Name);
            Assert.Equal("dave@userland.com", singleAuthor.Email);

            Assert.Equal("en-us", feed.Language);

            var singleItem = Assert.Single(feed.Items);

            Assert.NotNull(singleItem.Title);
            Assert.Equal("stuff", singleItem.Title.Text);
            Assert.Equal("http://bar", singleItem.Id);

            var singleLink = Assert.Single(singleItem.Links);
            Assert.NotNull(singleLink.Uri);
            Assert.Equal("http://bar/", singleLink.Uri.ToString());

            Assert.NotNull(singleItem.Summary);
            Assert.Equal("This is an article about some stuff", singleItem.Summary.Text);
        }

        [Fact]
        public void WriteTo_ShouldThrownNotSupportedException()
        {
            var formatter = new Rss091Formatter();

            Assert.Throws<NotSupportedException>(() => formatter.WriteTo(null!));
        }
    }
}
