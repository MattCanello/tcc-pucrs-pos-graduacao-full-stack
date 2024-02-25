using System.Diagnostics;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Telemetry
{
    internal static class ActivitySources
    {
        public static readonly ActivitySource CronPublishApp = new ActivitySource("MattCanello.NewsFeed.CronApi.CronPublishApp");
    }
}
