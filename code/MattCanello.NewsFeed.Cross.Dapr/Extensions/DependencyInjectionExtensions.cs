using MattCanello.NewsFeed.Cross.Dapr.Factories;
using MattCanello.NewsFeed.Cross.Dapr.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MattCanello.NewsFeed.Cross.Dapr.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtensions
    {
        public static void AddDapr(this IServiceCollection services)
        {
            services.AddSingleton<IBindingRequestFactory, BindingRequestFactory>();

            services.AddSingleton((sp) => new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            });

            services.AddDaprClient((builder) =>
            {
                var jsonSerializerOptions = services
                    .BuildServiceProvider()
                    .GetService<JsonSerializerOptions>();

                builder.UseJsonSerializationOptions(jsonSerializerOptions);
            });
        }
    }
}
