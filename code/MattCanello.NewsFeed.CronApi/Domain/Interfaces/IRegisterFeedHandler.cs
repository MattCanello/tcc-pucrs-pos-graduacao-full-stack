using MattCanello.NewsFeed.CronApi.Domain.Commands;
using MattCanello.NewsFeed.CronApi.Domain.Messages;

namespace MattCanello.NewsFeed.CronApi.Domain.Interfaces
{
    public interface IRegisterFeedHandler
    {
        Task<RegisterFeedResponseMessage> RegisterFeedAsync(RegisterFeedCommand registerFeedCommand, CancellationToken cancellationToken = default);
    }
}