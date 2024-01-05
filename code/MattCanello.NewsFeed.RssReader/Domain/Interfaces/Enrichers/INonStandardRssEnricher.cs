using MattCanello.NewsFeed.RssReader.Domain.Models;
using System.Xml.Linq;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.Enrichers
{
    public interface INonStandardRssEnricher
    {
        void Enrich(Entry entry, IEnumerable<XElement> xmlElements);
    }
}
