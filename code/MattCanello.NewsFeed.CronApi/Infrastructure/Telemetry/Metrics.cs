using System.Diagnostics.Metrics;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Telemetry
{
    internal static class Metrics
    {
        public static readonly Meter PublishedSlotsCount = new Meter("MattCanello.NewsFeed.CronApi.PublishedSlotsCount", "1.0.0");
    }
}
