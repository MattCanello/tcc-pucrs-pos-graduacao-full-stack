using Dapr.Client;
using MattCanello.NewsFeed.CronApi.Infrastructure.Interfaces;
using MattCanello.NewsFeed.Cross.Dapr.Interfaces;
using MattCanello.NewsFeed.Cross.Dapr.Extensions;

namespace MattCanello.NewsFeed.CronApi.Infrastructure.Factories
{
    public sealed class EventFactory : IEventFactory
    {
        private readonly IBindingRequestFactory _bindingRequestFactory;

        public EventFactory(IBindingRequestFactory bindingRequestFactory)
        {
            _bindingRequestFactory = bindingRequestFactory;
        }

        public BindingRequest CreateProcessRssEvent(string feedId, string bindingName)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(bindingName);

            var bindingRequest = _bindingRequestFactory
                .CreateBindingRequest(new { feedId }, bindingName);

            bindingRequest
                .SetFeedId(feedId);

            return bindingRequest;
        }
    }
}
