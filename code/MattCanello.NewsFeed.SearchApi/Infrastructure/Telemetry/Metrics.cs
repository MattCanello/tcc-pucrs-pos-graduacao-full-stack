using System.Diagnostics.Metrics;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Telemetry
{
    internal static class Metrics
    {
        public static readonly Meter IndexedDocuments = new Meter("MattCanello.NewsFeed.SearchApi.IndexedDocuments", "1.0.0");
    }
}
