using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Decorators
{
    public sealed class ChannelAppLogDecorator : IChannelApp
    {
        private readonly IChannelApp _app;
        private readonly ILogger _logger;

        public ChannelAppLogDecorator(IChannelApp app, ILogger<ChannelAppLogDecorator> logger)
        {
            _app = app;
            _logger = logger;
        }

        public async Task<Channel> CreateAsync(CreateChannelCommand command, CancellationToken cancellationToken = default)
        {
            using var state = _logger.BeginScope(command);

            _logger.LogInformation("Starting creating channel '{channelId}'", command.ChannelId);

            var channel = await _app.CreateAsync(command, cancellationToken);

            _logger.LogInformation("Finished creating channel '{channelId}'", command.ChannelId);

            return channel;
        }

        public async Task<Channel> UpdateAsync(UpdateChannelCommand command, CancellationToken cancellationToken = default)
        {
            using var state = _logger.BeginScope(command);

            _logger.LogInformation("Starting updating channel '{channelId}'", command.ChannelId);

            var channel = await _app.UpdateAsync(command, cancellationToken);

            _logger.LogInformation("Finished updating channel '{channelId}'", command.ChannelId);

            return channel;
        }
    }
}
