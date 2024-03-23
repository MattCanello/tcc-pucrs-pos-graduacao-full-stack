using Dapr.Client;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.Interfaces
{
    public interface IEventFactory
    {
        BindingRequest CreateNewFeedCreatedEvent(Feed feed, string bindingName);
    }
}
