using Dapr.Client;
using MattCanello.NewsFeed.Cross.Dapr.Interfaces;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Factories;
using MattCanello.NewsFeed.Cross.Dapr.Extensions;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Factories
{
    public sealed class EventFactory : IEventFactory
    {
        private readonly IBindingRequestFactory _bindingRequestFactory;

        public EventFactory(IBindingRequestFactory bindingRequestFactory)
        {
            _bindingRequestFactory = bindingRequestFactory;
        }

        public BindingRequest CreateNewEntryFoundEvent(string feedId, string channelId, Entry entry, string bindingName)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(entry);
            ArgumentNullException.ThrowIfNull(bindingName);

            var bindingRequest = _bindingRequestFactory
                .CreateBindingRequest(new { feedId, channelId, entry }, bindingName);

            bindingRequest
                .SetFeedId(feedId);

            return bindingRequest;
        }

        public BindingRequest CreateFeedConsumedEvent(string feedId, DateTimeOffset consumedDate, Channel channel, string bindingName)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(channel);
            ArgumentNullException.ThrowIfNull(bindingName);

            var bindingRequest = _bindingRequestFactory
                .CreateBindingRequest(new { feedId, consumedDate, channel }, bindingName);

            bindingRequest
                .SetFeedId(feedId);

            return bindingRequest;
        }
    }
}
