using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Mocks
{
    public class MockedFeedRepository : IFeedRepository
    {
        private readonly Func<string, (Feed, Channel)> _getFeedAndChannelFunc;

        public MockedFeedRepository(Func<string, (Feed, Channel)> getFeedAndChannelFunc) 
            => _getFeedAndChannelFunc = getFeedAndChannelFunc;

        public Task<(Feed, Channel)> GetFeedAndChannelAsync(string feedId, CancellationToken cancellationToken = default)
        {
            var data = _getFeedAndChannelFunc(feedId);

            return Task.FromResult(data);
        }
    }
}
