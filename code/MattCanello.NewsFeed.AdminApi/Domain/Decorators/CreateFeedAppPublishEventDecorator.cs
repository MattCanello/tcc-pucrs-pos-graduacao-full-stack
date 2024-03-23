using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Decorators
{
    public sealed class CreateFeedAppPublishEventDecorator : ICreateFeedApp
    {
        private readonly ICreateFeedApp _app;
        private readonly IEventPublisher _eventPublisher;

        public CreateFeedAppPublishEventDecorator(ICreateFeedApp app, IEventPublisher eventPublisher)
        {
            _app = app;
            _eventPublisher = eventPublisher;
        }

        public async Task<Feed> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createFeedCommand);

            var feed = await _app.CreateFeedAsync(createFeedCommand, cancellationToken);

            await _eventPublisher.PublishFeedCreatedEventAsync(feed, cancellationToken);

            return feed;
        }
    }
}
