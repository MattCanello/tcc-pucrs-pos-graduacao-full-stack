using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Decorators
{
    public sealed class UpdateChannelAppLogDecorator : IUpdateChannelApp
    {
        private readonly IUpdateChannelApp _app;
        private readonly ILogger _logger;

        public UpdateChannelAppLogDecorator(IUpdateChannelApp app, ILogger<UpdateChannelAppLogDecorator> logger)
        {
            _app = app;
            _logger = logger;
        }

        public async Task<Channel> UpdateChannelAsync(UpdateChannelCommand command, CancellationToken cancellationToken = default)
        {
            using var state = _logger.BeginScope(command);

            _logger.LogInformation("Starting updating channel of '{feedId}'", command.FeedId);

            var channel = await _app.UpdateChannelAsync(command, cancellationToken);

            _logger.LogInformation("Finished updating channel of '{feedId}'", command.FeedId);

            return channel;
        }
    }
}
