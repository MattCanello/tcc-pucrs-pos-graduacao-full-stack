using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Decorators
{
    public sealed class ChannelAppMetricsDecorator : IChannelApp
    {
        private readonly IChannelApp _channelApp;

        public ChannelAppMetricsDecorator(IChannelApp channelApp) 
            => _channelApp = channelApp;

        public async Task<Channel> CreateAsync(CreateChannelCommand command, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySources.ChannelApp.StartActivity("CreateAsync");

            var response = await _channelApp.CreateAsync(command, cancellationToken);

            activity?.SetTag("channelId", command.ChannelId);

            return response;
        }

        public async Task<Channel> UpdateAsync(UpdateChannelCommand command, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySources.ChannelApp.StartActivity("UpdateAsync");

            var response = await _channelApp.UpdateAsync(command, cancellationToken);

            activity?.SetTag("channelId", command.ChannelId);

            return response;
        }
    }
}
