using System.Diagnostics;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Telemetry
{
    internal static class ActivitySources
    {
        public static readonly ActivitySource FeedApp = new ActivitySource("MattCanello.NewsFeed.AdminApi.FeedApp");
        public static readonly ActivitySource ChannelApp = new ActivitySource("MattCanello.NewsFeed.AdminApi.ChannelApp");
    }
}
