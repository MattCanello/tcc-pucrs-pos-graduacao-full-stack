using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;

namespace MattCanello.NewsFeed.AdminApi.Domain.Application
{
    public sealed class CreateFeedApp : ICreateFeedApp
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IMapper _mapper;
        private readonly IChannelService _channelService;

        public CreateFeedApp(IFeedRepository feedRepository, IMapper mapper, IChannelService channelService)
        {
            _feedRepository = feedRepository;
            _mapper = mapper;
            _channelService = channelService;
        }

        public async Task<Feed> CreateFeedAsync(CreateFeedCommand createFeedCommand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createFeedCommand);

            await CheckIfFeedExistsAsync(createFeedCommand.FeedId!, cancellationToken);

            var feed = _mapper.Map<Feed>(createFeedCommand);

            feed.Channel = await _channelService.GetOrCreateAsync(createFeedCommand.ChannelId!, createFeedCommand.Data, cancellationToken);

            feed = await _feedRepository.CreateAsync(feed, cancellationToken);

            return feed;
        }

        private async Task CheckIfFeedExistsAsync(string feedId, CancellationToken cancellationToken = default)
        {
            var feed = await _feedRepository.GetByIdAsync(feedId, cancellationToken);

            if (feed is null)
                return;

            throw new FeedAlreadyExistingException(feedId);
        }
    }
}
