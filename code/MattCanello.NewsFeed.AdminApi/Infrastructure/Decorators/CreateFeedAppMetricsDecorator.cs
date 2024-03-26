using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Telemetry;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Decorators
{
    public sealed class CreateFeedAppMetricsDecorator : ICreateFeedApp
    {
        private readonly ICreateFeedApp _innerApp;

        public CreateFeedAppMetricsDecorator(ICreateFeedApp innerApp) 
            => _innerApp = innerApp;

        public async Task<Feed> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySources.CreateFeedApp.StartActivity("CreateFeedActivity");

            var response = await _innerApp.CreateFeedAsync(createFeedCommand, cancellationToken);

            activity?
                .SetTag("feedId", createFeedCommand.FeedId)
                .SetTag("channelId", createFeedCommand.ChannelId);

            return response;
        }
    }
}
