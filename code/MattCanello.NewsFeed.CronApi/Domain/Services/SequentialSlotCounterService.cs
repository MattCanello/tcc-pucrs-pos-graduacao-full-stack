using MattCanello.NewsFeed.CronApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Domain.Services
{
    public sealed class SequentialSlotCounterService : ISlotCounterService
    {
        private readonly ISlotRepository _slotRepository;

        public SequentialSlotCounterService(ISlotRepository slotRepository)
        {
            _slotRepository = slotRepository;
        }

        public async Task<byte> GetNextSlotAsync(CancellationToken cancellationToken = default)
        {
            var currentSlot = await _slotRepository.GetCurrentSlotAsync(cancellationToken);
            var nextSlot = CalculateNextSlot(currentSlot);
            await _slotRepository.SetCurrentSlotAsync(nextSlot, cancellationToken);
            return nextSlot;
        }

        private static byte CalculateNextSlot(byte currentSlot)
        {
            if (currentSlot >= 59)
                return 0;

            return (byte)(currentSlot + 1);
        }
    }
}
