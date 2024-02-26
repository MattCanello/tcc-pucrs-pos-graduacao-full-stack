using MattCanello.NewsFeed.Cross.Dapr.Factories;
using MattCanello.NewsFeed.Cross.Dapr.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.Cross.Dapr.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddDapr(this IServiceCollection services)
        {
            services.AddSingleton<IBindingRequestFactory, BindingRequestFactory>();

            services.AddDaprClient((builder) =>
            {
                builder.UseJsonSerializationOptions(new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                });
            });
        }
    }
}
