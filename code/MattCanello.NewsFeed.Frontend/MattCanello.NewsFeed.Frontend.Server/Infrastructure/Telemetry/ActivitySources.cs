using System.Diagnostics;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Telemetry
{
    internal static class ActivitySources
    {
        public static readonly ActivitySource ArticleApp = new ActivitySource("MattCanello.NewsFeed.Frontend.ArticleApp");
        public static readonly ActivitySource NewEntryHandler = new ActivitySource("MattCanello.NewsFeed.Frontend.NewEntryHandler");
    }
}
