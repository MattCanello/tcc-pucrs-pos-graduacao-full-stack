using Dapr.Client;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Factories
{
    public interface IEventFactory
    {
        BindingRequest CreateNewEntryFoundEvent(string feedId, Entry entry, string bindingName);
        BindingRequest CreateFeedConsumedEvent(string feedId, DateTimeOffset consumedDate, Channel channel, string bindingName);
    }
}
