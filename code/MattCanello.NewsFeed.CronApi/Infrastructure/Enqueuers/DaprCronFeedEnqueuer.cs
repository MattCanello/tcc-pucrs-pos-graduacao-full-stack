using Dapr.Client;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Enqueuers
{
    public sealed class DaprCronFeedEnqueuer : ICronFeedEnqueuer
    {
        private readonly DaprClient _daprClient;
        const string BindingName = "rsspublishcommands";

        public DaprCronFeedEnqueuer(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public Task EnqueueFeedToProcessAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            // TODO: implementar

            return Task.CompletedTask;
        }
    }
}
