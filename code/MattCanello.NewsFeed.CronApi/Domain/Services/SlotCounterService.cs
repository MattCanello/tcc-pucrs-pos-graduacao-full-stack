using MattCanello.NewsFeed.CronApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Domain.Services
{
    public sealed class SlotCounterService : ISlotCounterService
    {
        private readonly ISlotRepository _slotRepository;

        public SlotCounterService(ISlotRepository slotRepository)
        {
            _slotRepository = slotRepository;
        }

        public async Task<byte> GetNextSlotAsync()
        {
            var currentSlot = await _slotRepository.GetCurrentSlotAsync();
            var nextSlot = CalculateNextSlot(currentSlot);
            await _slotRepository.SetCurrentSlotAsync(nextSlot);
            return nextSlot;
        }

        private static byte CalculateNextSlot(byte currentSlot)
        {
            if (currentSlot < 0 || currentSlot >= 59)
                return 0;

            return currentSlot++;
        }
    }
}
