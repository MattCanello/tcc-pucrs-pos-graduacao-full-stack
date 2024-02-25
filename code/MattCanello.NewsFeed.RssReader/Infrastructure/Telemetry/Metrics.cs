using System.Diagnostics.Metrics;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Telemetry
{
    internal static class Metrics
    {
        public static readonly Meter PublishedEntriesCount = new Meter("MattCanello.NewsFeed.RssReader.PublishedEntriesCount", "1.0.0");
    }
}
