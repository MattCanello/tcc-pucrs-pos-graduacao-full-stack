using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Domain.Responses;

namespace MattCanello.NewsFeed.AdminApi.Tests.Mocks
{
    sealed class MockedChannelRepository : IChannelRepository
    {
        private readonly IDictionary<string, Channel> _data;

        public MockedChannelRepository() 
            : this(data: null) { }

        public MockedChannelRepository(Channel channel)
            : this(new Dictionary<string, Channel>(capacity: 1) { { channel.ChannelId, channel } }) { }

        public MockedChannelRepository(IDictionary<string, Channel>? data = null)
            => _data = data ?? new Dictionary<string, Channel>();

        public Channel? this[string channelId]
        {
            get
            {
                if (_data.TryGetValue(channelId, out var channel))
                    return channel;

                return null;
            }
        }

        public int Count => _data.Count;

        public Task<Channel> CreateAsync(Channel channel, CancellationToken cancellationToken = default)
        {
            _data[channel.ChannelId] = channel;

            return Task.FromResult(channel);
        }

        public Task<Channel?> GetByIdAsync(string channelId, CancellationToken cancellationToken = default)
        {
            if (_data.TryGetValue(channelId, out var channel))
                return Task.FromResult<Channel?>(channel);

            return Task.FromResult<Channel?>(null);
        }

        public Task<QueryResponse<Channel>> QueryAsync(QueryCommand command, CancellationToken cancellationToken = default)
        {
            var results = _data.Values
                .OrderBy(item => item.ChannelId)
                .Skip(command.Skip ?? 0)
                .Take(command.PageSize ?? QueryCommand.DefaultPageSize);

            return Task.FromResult(new QueryResponse<Channel>(_data.Count, results));
        }

        public Task<Channel> UpdateAsync(Channel channel, CancellationToken cancellationToken = default)
        {
            _data[channel.ChannelId] = channel;

            return Task.FromResult(channel);
        }
    }
}
