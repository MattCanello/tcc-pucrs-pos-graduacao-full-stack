using System.Diagnostics.Metrics;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Telemetry
{
    internal static class Metrics
    {
        public static readonly Meter FrontPageHits = new Meter("MattCanello.NewsFeed.Frontend.FrontPageHits", "1.0.0");
        public static readonly Meter ArticleDetailsHits = new Meter("MattCanello.NewsFeed.Frontend.ArticleDetailsHits", "1.0.0");
        public static readonly Meter ChannelHits = new Meter("MattCanello.NewsFeed.Frontend.ChannelHits", "1.0.0");
        public static readonly Meter SearchHits = new Meter("MattCanello.NewsFeed.Frontend.SearchHits", "1.0.0");
    }
}
