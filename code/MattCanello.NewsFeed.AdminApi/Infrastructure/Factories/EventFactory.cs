using Dapr.Client;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Infrastructure.Interfaces;
using MattCanello.NewsFeed.Cross.Dapr.Extensions;
using MattCanello.NewsFeed.Cross.Dapr.Interfaces;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Factories
{
    public sealed class EventFactory : IEventFactory
    {
        private readonly IBindingRequestFactory _bindingRequestFactory;

        public EventFactory(IBindingRequestFactory bindingRequestFactory)
        {
            _bindingRequestFactory = bindingRequestFactory;
        }

        public BindingRequest CreateNewFeedCreatedEvent(FeedWithChannel feed, string bindingName)
        {
            ArgumentNullException.ThrowIfNull(feed);
            ArgumentNullException.ThrowIfNull(bindingName);

            var bindingRequest = _bindingRequestFactory
                .CreateBindingRequest(new { feedId = feed.FeedId, url = feed.Url, channelId = feed.Channel?.ChannelId }, bindingName);

            bindingRequest
                .SetFeedId(feed.FeedId);

            return bindingRequest;
        }
    }
}
