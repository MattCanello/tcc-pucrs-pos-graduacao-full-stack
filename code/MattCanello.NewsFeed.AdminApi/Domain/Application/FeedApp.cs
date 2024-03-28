using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;

namespace MattCanello.NewsFeed.AdminApi.Domain.Application
{
    public sealed class FeedApp : IFeedApp
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IChannelService _channelService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;

        public FeedApp(IFeedRepository feedRepository, IChannelService channelService, IDateTimeProvider dateTimeProvider, IMapper mapper)
        {
            _feedRepository = feedRepository;
            _channelService = channelService;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }

        public async Task<FeedWithChannel> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createFeedCommand);

            await CheckIfFeedExistsAsync(createFeedCommand.FeedId!, cancellationToken);

            var feed = _mapper.Map<FeedWithChannel>(createFeedCommand);

            feed.CreatedAt = _dateTimeProvider.GetUtcNow();

            feed.Channel = await _channelService.GetOrCreateAsync(createFeedCommand.ChannelId!, cancellationToken);

            feed = await _feedRepository.CreateAsync(feed, cancellationToken);

            return feed;
        }

        public async Task<FeedWithChannel> UpdateFeedAsync(UpdateFeedCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var feed = await _feedRepository.GetByIdAsync(command.FeedId!, cancellationToken)
                ?? throw new FeedNotFoundException(command.FeedId!);

            feed = _mapper.Map(command.Channel, feed);

            feed = await _feedRepository.UpdateAsync(feed, cancellationToken);

            if (feed.Channel != null)
                feed.Channel = await _channelService.UpdateChannelAsync(feed.Channel.ChannelId, command.Channel!, cancellationToken);

            return feed;
        }

        private async Task CheckIfFeedExistsAsync(string feedId, CancellationToken cancellationToken = default)
        {
            var feed = await _feedRepository.GetByIdAsync(feedId, cancellationToken);

            if (feed is null)
                return;

            throw new FeedAlreadyExistsException(feedId);
        }
    }
}
