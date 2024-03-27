using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    sealed class MockedCreateFeedApp : IFeedApp
    {
        private readonly Func<CreateFeedCommand, Feed> _createFeedFunc;
        private readonly Func<UpdateFeedCommand, Feed> _updateFeedFunc;

        public MockedCreateFeedApp(Func<CreateFeedCommand, Feed> createFeedFunc, Func<UpdateFeedCommand, Feed> updateFeedFunc)
        {
            _createFeedFunc = createFeedFunc ?? throw new ArgumentNullException(nameof(createFeedFunc));
            _updateFeedFunc = updateFeedFunc ?? throw new ArgumentNullException(nameof(updateFeedFunc));
        }

        public Task<Feed> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default)
        {
            var feed = _createFeedFunc(createFeedCommand);

            return Task.FromResult(feed);
        }

        public Task<Feed> UpdateFeedAsync(UpdateFeedCommand command, CancellationToken cancellationToken = default)
        {
            var feed = _updateFeedFunc(command);

            return Task.FromResult(feed);
        }
    }
}
