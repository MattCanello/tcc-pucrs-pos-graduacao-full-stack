using System.ServiceModel.Syndication;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Formatters.Rss091
{
    public class Rss091Formatter : SyndicationFeedFormatter
    {
        public override string Version => "0.91";
        private SyndicationFeed? feed;

        public override bool CanRead(XmlReader reader)
        {
            var isRss = reader.IsStartElement("rss");
            var isSupportedVersion = reader.GetAttribute("version") == Version;
            return isRss && isSupportedVersion;
        }

        public override void ReadFrom(XmlReader reader)
        {
            var feed = Feed ?? CreateFeedInstance();
            SetFeed(feed);

            var items = new List<SyndicationItem>();
            var authors = new HashSet<Rss091EmailAndPersonString>();

            reader.ReadStartElement(); // <rss>

            reader.ReadStartElement("channel");
            while (reader.IsStartElement())
            {
                if (reader.IsStartElement("title"))
                    feed.Title = new TextSyndicationContent(reader.ReadElementContentAsString());
                else if (reader.IsStartElement("link"))
                    feed.Links.Add(new SyndicationLink(new Uri(reader.ReadElementContentAsString())));
                else if (reader.IsStartElement("description"))
                    feed.Description = new TextSyndicationContent(reader.ReadElementContentAsString());
                else if (reader.IsStartElement("language"))
                    feed.Language = reader.ReadElementContentAsString();
                else if (reader.IsStartElement("copyright"))
                    feed.Copyright = new TextSyndicationContent(reader.ReadElementContentAsString());
                else if (reader.IsStartElement("docs"))
                    feed.Documentation = new SyndicationLink(new Uri(reader.ReadElementContentAsString()));
                else if (reader.IsStartElement("pubDate"))
                    feed.LastUpdatedTime = DateTimeOffset.Parse(reader.ReadElementContentAsString());
                else if (reader.IsStartElement("webMaster"))
                {
                    if (Rss091EmailAndPersonString.TryParse(reader.ReadElementContentAsString(), out var emailAndPerson))
                        authors.Add(emailAndPerson!);
                }
                else if (reader.IsStartElement("managingEditor"))
                {
                    if (Rss091EmailAndPersonString.TryParse(reader.ReadElementContentAsString(), out var emailAndPerson))
                        authors.Add(emailAndPerson!);
                }
                else if (reader.IsStartElement("image"))
                {
                    reader.ReadStartElement();
                    while (reader.IsStartElement())
                    {
                        if (reader.IsStartElement("url"))
                            feed.ImageUrl = new Uri(reader.ReadElementContentAsString());
                        else
                            reader.Skip();
                    }
                    reader.ReadEndElement(); // </image>
                }
                else if (reader.IsStartElement("item") && TryReadItem(reader, out var item))
                    items.Add(item!);
                else
                    reader.Skip();
            }

            reader.ReadEndElement(); // </channel>
            reader.ReadEndElement(); // </rss>

            feed.Items = items.AsEnumerable();
            foreach (var author in authors)
                feed.Authors.Add(new SyndicationPerson(author.Email, author.DisplayName, null));
        }

        public override void WriteTo(XmlWriter writer)
        {
            throw new NotSupportedException();
        }

        protected override SyndicationFeed CreateFeedInstance()
        {
            feed = new SyndicationFeed();
            return feed;
        }

        private static bool TryReadItem(XmlReader reader, out SyndicationItem? item)
        {
            ArgumentNullException.ThrowIfNull(reader);

            reader.ReadStartElement(); // <item>
            item = new SyndicationItem();

            while (reader.IsStartElement())
            {
                if (reader.IsStartElement("title"))
                    item.Title = new TextSyndicationContent(reader.ReadElementContentAsString());
                else if (reader.IsStartElement("link"))
                {
                    var link = reader.ReadElementContentAsString();
                    item.Links.Add(new SyndicationLink(new Uri(link)));
                    item.Id = link;
                }
                else if (reader.IsStartElement("description"))
                    item.Summary = new TextSyndicationContent(reader.ReadElementContentAsString());
                else if (reader.IsStartElement("pubDate"))
                    item.PublishDate = DateTimeOffset.Parse(reader.ReadElementContentAsString());
                else if (reader.IsStartElement("author"))
                {
                    var authorName = reader.ReadElementContentAsString();

                    if (!string.IsNullOrEmpty(authorName))
                        item.Authors.Add(new SyndicationPerson(null, authorName, null));
                }
                else
                    reader.Skip();
            }

            reader.ReadEndElement(); // </item>

            return !string.IsNullOrEmpty(item.Id);
        }
    }
}
