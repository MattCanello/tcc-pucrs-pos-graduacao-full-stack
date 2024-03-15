using System.Diagnostics;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.Telemetry
{
    internal static class ActivitySources
    {
        public static readonly ActivitySource IndexApp = new ActivitySource("MattCanello.NewsFeed.SearchApi.IndexApp");
        public static readonly ActivitySource SearchApp = new ActivitySource("MattCanello.NewsFeed.SearchApi.SearchApp");
    }
}
