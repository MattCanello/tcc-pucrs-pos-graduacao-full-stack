using MattCanello.NewsFeed.RssReader.Interfaces;
using MattCanello.NewsFeed.RssReader.Models;
using System.Xml.Linq;

namespace MattCanello.NewsFeed.RssReader.Enrichers
{
    public sealed class MediaEnricher : INonStandardRssEnricher
    {
        public static readonly XNamespace MediaXNamespace = "http://search.yahoo.com/mrss/";

        public void Enrich(Entry entry, IEnumerable<XElement> xmlElements)
        {
            ArgumentNullException.ThrowIfNull(entry);
            ArgumentNullException.ThrowIfNull(xmlElements);

            foreach (var xmlElement in xmlElements)
            {
                switch (xmlElement.Name.LocalName)
                {
                    case "content":
                        TryParseContent(entry, xmlElement);
                        break;
                }
            }
        }

        private static void TryParseContent(Entry entry, XElement xmlElement)
        {
            ArgumentNullException.ThrowIfNull(entry);
            ArgumentNullException.ThrowIfNull(xmlElement);

            var thumbnail = TryParseThumbnail(xmlElement);
            if (thumbnail != null)
                entry.Thumbnails.Add(thumbnail);
        }

        private static Thumbnail? TryParseThumbnail(XElement xmlElement)
        {
            ArgumentNullException.ThrowIfNull(xmlElement);

            string? url = xmlElement.Attribute("url")?.Value;
            if (string.IsNullOrEmpty(url))
                return null;

            var thumbnail = new Thumbnail()
            {
                Url = url
            };

            if (int.TryParse(xmlElement.Attribute("width")?.Value, out int width))
                thumbnail.Width = width;

            thumbnail.Credit = xmlElement.Element(MediaXNamespace + "credit")?.Value;
            return thumbnail;
        }
    }
}
