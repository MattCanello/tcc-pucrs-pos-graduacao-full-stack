using AutoMapper;
using MattCanello.NewsFeed.RssReader.Domain.Commands;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Handlers;
using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Repositories;
using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Handlers
{
    public sealed class CreateFeedHandler : ICreateFeedHandler
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IMapper _mapper;

        public CreateFeedHandler(IFeedRepository feedRepository, IMapper mapper)
        {
            _feedRepository = feedRepository;
            _mapper = mapper;
        }

        public async Task<Feed> CreateFeedAsync(CreateFeedCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var feed = _mapper.Map<Feed>(command);
            await _feedRepository.UpdateAsync(feed.FeedId, feed);

            return feed;
        }
    }
}
