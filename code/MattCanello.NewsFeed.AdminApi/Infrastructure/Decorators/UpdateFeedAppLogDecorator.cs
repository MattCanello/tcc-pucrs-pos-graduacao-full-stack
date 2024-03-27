using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Decorators
{
    public sealed class UpdateFeedAppLogDecorator : IUpdateFeedApp
    {
        private readonly IUpdateFeedApp _app;
        private readonly ILogger _logger;

        public UpdateFeedAppLogDecorator(IUpdateFeedApp app, ILogger<UpdateFeedAppLogDecorator> logger)
        {
            _app = app;
            _logger = logger;
        }

        public async Task<Feed> UpdateFeedAsync(UpdateFeedCommand command, CancellationToken cancellationToken = default)
        {
            using var state = _logger.BeginScope(command);

            _logger.LogInformation("Starting updating feed '{feedId}'", command.FeedId);

            var feed = await _app.UpdateFeedAsync(command, cancellationToken);

            _logger.LogInformation("Finished updating feed '{feedId}'", command.FeedId);

            return feed;
        }
    }
}
