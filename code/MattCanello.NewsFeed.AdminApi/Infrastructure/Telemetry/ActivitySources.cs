using System.Diagnostics;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Telemetry
{
    internal static class ActivitySources
    {
        public static readonly ActivitySource CreateFeedApp = new ActivitySource("MattCanello.NewsFeed.AdminApi.CreateFeedApp");
        public static readonly ActivitySource UpdateFeedApp = new ActivitySource("MattCanello.NewsFeed.AdminApi.UpdateFeedApp");
        public static readonly ActivitySource ChannelApp = new ActivitySource("MattCanello.NewsFeed.AdminApi.ChannelApp");
    }
}
