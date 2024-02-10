using MattCanello.NewsFeed.CronApi.Domain.Commands;
using MattCanello.NewsFeed.CronApi.Domain.Interfaces;
using MattCanello.NewsFeed.CronApi.Domain.Messages;
using MattCanello.NewsFeed.CronApi.Domain.Models;

namespace MattCanello.NewsFeed.CronApi.Domain.Handlers
{
    public sealed class RegisterFeedHandler : IRegisterFeedHandler
    {
        private readonly ISlotCounterService _slotCounterService;
        private readonly IFeedRepository _feedRepository;

        public RegisterFeedHandler(ISlotCounterService slotCounterService, IFeedRepository feedRepository)
        {
            _slotCounterService = slotCounterService;
            _feedRepository = feedRepository;
        }

        public async Task<RegisterFeedResponseMessage> RegisterFeedAsync(RegisterFeedCommand registerFeedCommand, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(registerFeedCommand);

            var slot = await _slotCounterService.GetNextSlotAsync();

            var feed = new Feed() { FeedId = registerFeedCommand.FeedId! };

            await _feedRepository.PlaceFeedAsync(slot, feed, cancellationToken);

            return new RegisterFeedResponseMessage(slot, feed);
        }
    }
}
