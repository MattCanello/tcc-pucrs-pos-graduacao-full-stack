using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Mocks
{
    sealed class MockedAdminClient : IAdminClient
    {
        private readonly Func<string, AdminFeedWithChannel> _getFeedFunc;
        private readonly Func<AdminQueryCommand, AdminQueryResponse<AdminChannel>> _searchFunc;

        public MockedAdminClient(Func<string, AdminFeedWithChannel> getFeedFunc)
        {
            _getFeedFunc = getFeedFunc;
            _searchFunc = (cmd) => throw new NotImplementedException();
        }

        public MockedAdminClient(Func<AdminQueryCommand, AdminQueryResponse<AdminChannel>> searchFunc)
        {
            _getFeedFunc = (feedId) => throw new NotImplementedException();
            _searchFunc = searchFunc;
        }

        public Task<AdminFeedWithChannel> GetFeedAsync(string feedId, CancellationToken cancellationToken = default)
        {
            var adminFeedWithChannel = _getFeedFunc(feedId);

            return Task.FromResult(adminFeedWithChannel);
        }

        public Task<AdminQueryResponse<AdminChannel>> QueryChannelsAsync(AdminQueryCommand queryCommand, CancellationToken cancellationToken = default)
        {
            var queryResponse = _searchFunc(queryCommand);

            return Task.FromResult(queryResponse);
        }
    }
}
