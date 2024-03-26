using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Decorators
{
    public sealed class UpdateChannelAppMetricsDecorator : IUpdateChannelApp
    {
        private readonly IUpdateChannelApp _innerApp;

        public UpdateChannelAppMetricsDecorator(IUpdateChannelApp innerApp) 
            => _innerApp = innerApp;

        public async Task<Channel> UpdateChannelAsync(UpdateChannelCommand command, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySources.UpdateChannelApp.StartActivity("UpdateChannelActivity");

            var response = await _innerApp.UpdateChannelAsync(command, cancellationToken);

            activity?.SetTag("feedId", command.FeedId);

            return response;
        }
    }
}
