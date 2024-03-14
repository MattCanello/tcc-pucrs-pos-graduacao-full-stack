using System.Diagnostics.Metrics;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Telemetry
{
    internal static class Metrics
    {
        public static readonly Meter IndexedDocuments = new Meter("MattCanello.NewsFeed.SearchApi.IndexedDocuments", "1.0.0");
        public static readonly Meter SearchCount = new Meter("MattCanello.NewsFeed.SearchApi.SearchCount", "1.0.0");
        public static readonly Meter EmptySearchCount = new Meter("MattCanello.NewsFeed.SearchApi.EmptySearchCount", "1.0.0");
        public static readonly Meter SearchSpeed = new Meter("MattCanello.NewsFeed.SearchApi.SearchSpeed", "1.0.0");
    }
}
