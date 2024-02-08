using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Enrichers;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using System.Xml.Linq;

namespace MattCanello.NewsFeed.RssReader.Domain.Enrichers
{
    public sealed class PurlEnricher : INonStandardRssEnricher
    {
        public static readonly XNamespace PurlXNamespace = "http://purl.org/dc/elements/1.1/";

        public void Enrich(Entry entry, IEnumerable<XElement> xmlElements)
        {
            ArgumentNullException.ThrowIfNull(entry);
            ArgumentNullException.ThrowIfNull(xmlElements);

            foreach (var xmlElement in xmlElements)
            {
                switch (xmlElement.Name.LocalName)
                {
                    case "creator":
                        TryParseCreator(entry, xmlElement);
                        break;
                }
            }
        }

        private static void TryParseCreator(Entry entryItem, XElement xmlElement)
        {
            ArgumentNullException.ThrowIfNull(entryItem);
            ArgumentNullException.ThrowIfNull(xmlElement);

            var creator = xmlElement.Value;
            if (string.IsNullOrEmpty(creator))
                return;

            var author = new Author(creator);
            entryItem.Authors.Add(author);
        }
    }
}
