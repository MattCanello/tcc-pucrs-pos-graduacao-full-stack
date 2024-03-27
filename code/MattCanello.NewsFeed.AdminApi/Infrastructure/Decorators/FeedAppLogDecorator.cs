using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Decorators
{
    public sealed class FeedAppLogDecorator : IFeedApp
    {
        private readonly IFeedApp _app;
        private readonly ILogger _logger;

        public FeedAppLogDecorator(IFeedApp app, ILogger<FeedAppLogDecorator> logger)
        {
            _app = app;
            _logger = logger;
        }

        public async Task<Feed> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default)
        {
            using var state = _logger.BeginScope(createFeedCommand);

            _logger.LogInformation("Starting creating feed '{feedId}'", createFeedCommand.FeedId);

            var feed = await _app.CreateFeedAsync(createFeedCommand, cancellationToken);

            _logger.LogInformation("Finished creating feed '{feedId}'", createFeedCommand.FeedId);

            return feed;
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
