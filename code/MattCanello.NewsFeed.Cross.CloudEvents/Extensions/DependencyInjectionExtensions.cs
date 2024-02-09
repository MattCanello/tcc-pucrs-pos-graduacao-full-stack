using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;
using MattCanello.NewsFeed.Cross.CloudEvents.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.Cross.CloudEvents.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCloudEvents(this IServiceCollection services)
        {
            services
                .AddSingleton<CloudEventFormatter, JsonEventFormatter>((s) =>
                {
                    return new JsonEventFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                    }, new JsonDocumentOptions());
                })
                .AddSingleton<CloudEventJsonInputFormatter>();
        }
    }
}
