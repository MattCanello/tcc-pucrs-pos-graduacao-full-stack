using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Decorators
{
    public sealed class FeedAppMetricsDecorator : IFeedApp
    {
        private readonly IFeedApp _innerApp;

        public FeedAppMetricsDecorator(IFeedApp innerApp) 
            => _innerApp = innerApp;

        public async Task<Feed> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySources.FeedApp.StartActivity("CreateFeedActivity");

            var response = await _innerApp.CreateFeedAsync(createFeedCommand, cancellationToken);

            activity?
                .SetTag("feedId", createFeedCommand.FeedId)
                .SetTag("channelId", createFeedCommand.ChannelId);

            return response;
        }

        public async Task<Feed> UpdateFeedAsync(UpdateFeedCommand command, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySources.FeedApp.StartActivity("UpdateFeedActivity");

            var feed = await _innerApp.UpdateFeedAsync(command, cancellationToken);

            activity?.SetTag("feedId", command.FeedId);

            return feed;
        }
    }
}
