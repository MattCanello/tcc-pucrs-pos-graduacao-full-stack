using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Interfaces
{
    public interface IChannelPublisher
    {
        Task PublishChannelUpdatedAsync(string feedId, Channel channel, CancellationToken cancellationToken = default);
    }
}
