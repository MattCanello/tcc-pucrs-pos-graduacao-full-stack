using MattCanello.NewsFeed.CronApi.Domain.Interfaces;

namespace MattCanello.NewsFeed.CronApi.Tests.Stubs
{
    public sealed class SlotRepositoryStub : ISlotRepository
    {
        private byte _currentSlot;

        public SlotRepositoryStub(byte currentSlot) => _currentSlot = currentSlot;

        public Task<byte> GetCurrentSlotAsync(CancellationToken cancellationToken = default) => Task.FromResult(_currentSlot);

        public Task SetCurrentSlotAsync(byte slot, CancellationToken cancellationToken = default)
        {
            _currentSlot = slot;
            return Task.CompletedTask;
        }
    }
}
