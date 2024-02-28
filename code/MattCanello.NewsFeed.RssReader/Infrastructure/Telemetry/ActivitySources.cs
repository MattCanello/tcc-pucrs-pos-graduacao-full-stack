using System.Diagnostics;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Telemetry
{
    internal static class ActivitySources
    {
        public static readonly ActivitySource RssApp = new ActivitySource("MattCanello.NewsFeed.RssReader.RssApp");
    }
}
