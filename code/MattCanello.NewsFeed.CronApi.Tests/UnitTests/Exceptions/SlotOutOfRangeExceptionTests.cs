using AutoFixture.Xunit2;
using MattCanello.NewsFeed.CronApi.Domain.Exceptions;

namespace MattCanello.NewsFeed.CronApi.Tests.UnitTests.Exceptions
{
    public class SlotOutOfRangeExceptionTests
    {
        [Theory, AutoData]
        public void Constructor_GivenAProvidedSlot_ShouldPreserveProvidedSlot(byte slot)
        {
            var exception = new SlotOutOfRangeException(slot);

            Assert.NotNull(exception.ProvidedSlot);
            Assert.Equal(slot, exception.ProvidedSlot);
        }

        [Theory]
        [InlineData(byte.MinValue)]
        [InlineData(1)]
        [InlineData(59)]
        public void ThrowIfOutOfRange_GivenAValidSlot_ShouldNotThrowException(byte slot)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);
        }

        [Theory]
        [InlineData(60)]
        [InlineData(61)]
        [InlineData(byte.MaxValue)]
        public void ThrowIfOutOfRange_GivenAValidSlot_ShouldThrowException(byte slot)
        {
            Assert.Throws<SlotOutOfRangeException>(() =>
                SlotOutOfRangeException.ThrowIfOutOfRange(slot)
            );
        }
    }
}
