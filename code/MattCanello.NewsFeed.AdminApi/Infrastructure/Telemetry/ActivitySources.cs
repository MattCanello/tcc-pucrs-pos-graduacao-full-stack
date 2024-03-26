using System.Diagnostics;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Telemetry
{
    internal static class ActivitySources
    {
        public static readonly ActivitySource CreateFeedApp = new ActivitySource("MattCanello.NewsFeed.AdminApi.CreateFeedApp");
        public static readonly ActivitySource UpdateChannelApp = new ActivitySource("MattCanello.NewsFeed.AdminApi.UpdateChannelApp");
    }
}
