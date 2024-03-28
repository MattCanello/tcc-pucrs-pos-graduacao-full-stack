using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Domain.Responses;
using MattCanello.NewsFeed.AdminApi.Tests.Profiles;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    sealed class MockedFeedRepository : IFeedRepository
    {
        private readonly IDictionary<string, FeedWithChannel> _data;
        private readonly IMapper _internalMapper = new MapperConfiguration(config => config.AddProfile<FeedWithChannelToFeedProfile>()).CreateMapper();

        public MockedFeedRepository() 
            : this(data: null) { }

        public MockedFeedRepository(FeedWithChannel feed)
            : this(new Dictionary<string, FeedWithChannel>(capacity: 1) { { feed.FeedId, feed } }) { }

        public MockedFeedRepository(IDictionary<string, FeedWithChannel>? data = null)
            => _data = data ?? new Dictionary<string, FeedWithChannel>();

        public Task<FeedWithChannel> CreateAsync(FeedWithChannel feed, CancellationToken cancellationToken = default)
        {
            _data[feed.FeedId] = feed;

            return Task.FromResult(feed);
        }

        public Task<FeedWithChannel?> GetByIdAsync(string feedId, CancellationToken cancellationToken = default)
        {
            if (_data.TryGetValue(feedId, out FeedWithChannel? feed))
                return Task.FromResult<FeedWithChannel?>(feed);

            return Task.FromResult<FeedWithChannel?>(null);
        }

        public Task<QueryResponse<Feed>> QueryAsync(QueryCommand command, CancellationToken cancellationToken = default)
        {
            var results = _data.Values
                .OrderBy(item => item.FeedId)
                .Select(item => _internalMapper.Map<Feed>(item))
                .Skip(command.Skip ?? 0)
                .Take(command.PageSize ?? QueryCommand.DefaultPageSize);

            return Task.FromResult(new QueryResponse<Feed>(_data.Count, results));
        }

        public Task<FeedWithChannel> UpdateAsync(FeedWithChannel feed, CancellationToken cancellationToken = default)
        {
            _data[feed.FeedId] = feed;

            return Task.FromResult(feed);
        }
    }
}
