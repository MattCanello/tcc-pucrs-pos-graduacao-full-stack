namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Configuration
{
    static class EnvironmentVariables
    {
        public const string FRONTPAGE_ARTICLE_COUNT = "FRONTPAGE_ARTICLE_COUNT";
        public const string CHANNEL_LIST_BULK_COUNT = "CHANNEL_LIST_BULK_COUNT";

        public const string FEED_EXPIRY_TIME = "FEED_EXPIRY_TIME";
        public const string CHANNEL_EXPIRY_TIME = "CHANNEL_EXPIRY_TIME";

        public const string FRONTEND_BASE_URL = "FRONTEND_BASE_URL";
        public const string DAPR_ENDPOINT = "DAPR_ENDPOINT";

        public static string FrontendBaseUrl() 
            => Environment.GetEnvironmentVariable(FRONTEND_BASE_URL) ?? "http://localhost:5173";

        public static string? DaprEndpoint()
            => Environment.GetEnvironmentVariable(DAPR_ENDPOINT);
    }
}
