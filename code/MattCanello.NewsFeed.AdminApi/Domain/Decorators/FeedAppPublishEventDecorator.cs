using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Decorators
{
    public sealed class FeedAppPublishEventDecorator : IFeedApp
    {
        private readonly IFeedApp _app;
        private readonly IEventPublisher _eventPublisher;

        public FeedAppPublishEventDecorator(IFeedApp app, IEventPublisher eventPublisher)
        {
            _app = app;
            _eventPublisher = eventPublisher;
        }

        public async Task<FeedWithChannel> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createFeedCommand);

            var feed = await _app.CreateFeedAsync(createFeedCommand, cancellationToken);

            await _eventPublisher.PublishFeedCreatedEventAsync(feed, cancellationToken);

            return feed;
        }

        public async Task<FeedWithChannel> UpdateFeedAsync(UpdateFeedCommand command, CancellationToken cancellationToken = default) 
            => await _app.UpdateFeedAsync(command, cancellationToken);
    }
}
