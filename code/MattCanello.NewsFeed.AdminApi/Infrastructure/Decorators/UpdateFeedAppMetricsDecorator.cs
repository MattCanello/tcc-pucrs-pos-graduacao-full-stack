using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Decorators
{
    public sealed class UpdateFeedAppMetricsDecorator : IUpdateFeedApp
    {
        private readonly IUpdateFeedApp _innerApp;

        public UpdateFeedAppMetricsDecorator(IUpdateFeedApp innerApp) 
            => _innerApp = innerApp;

        public async Task<Feed> UpdateFeedAsync(UpdateFeedCommand command, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySources.UpdateFeedApp.StartActivity("UpdateFeedActivity");

            var feed = await _innerApp.UpdateFeedAsync(command, cancellationToken);

            activity?.SetTag("feedId", command.FeedId);

            return feed;
        }
    }
}
