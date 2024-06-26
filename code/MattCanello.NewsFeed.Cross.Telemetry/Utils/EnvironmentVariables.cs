﻿namespace MattCanello.NewsFeed.Cross.Telemetry.Utils
{
    public static class EnvironmentVariables
    {
        public static bool HasApplicationInsightsConnectionString(out string? appInsightsConnStr)
        {
            appInsightsConnStr = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
            return !string.IsNullOrEmpty(appInsightsConnStr);
        }
    }
}
