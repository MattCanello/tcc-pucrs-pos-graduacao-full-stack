using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Decorators
{
    public sealed class CreateFeedAppLogDecorator : ICreateFeedApp
    {
        private readonly ICreateFeedApp _app;
        private readonly ILogger _logger;

        public CreateFeedAppLogDecorator(ICreateFeedApp app, ILogger<CreateFeedAppLogDecorator> logger)
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
    }
}
