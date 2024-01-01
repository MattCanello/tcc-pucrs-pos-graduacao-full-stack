using MattCanello.NewsFeed.RssReader.Models;
using System.Xml.Linq;

namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface INonStandardRssEnricher
    {
        void Enrich(Entry entry, IEnumerable<XElement> xmlElements);
    }
}
