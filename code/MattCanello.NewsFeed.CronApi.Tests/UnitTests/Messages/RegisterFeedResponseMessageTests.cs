using AutoFixture.Xunit2;
using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using MattCanello.NewsFeed.CronApi.Domain.Messages;
using MattCanello.NewsFeed.CronApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattCanello.NewsFeed.CronApi.Tests.UnitTests.Messages
{
    public class RegisterFeedResponseMessageTests
    {
        [Theory, AutoData]
        public void Constructor_GivenValidSlot_AndValidFeed_ShouldNotThrowException(Feed feed)
        {
            const byte validSlot = 1;

            var responseMessage = new RegisterFeedResponseMessage(validSlot, feed);
            
            Assert.Equal(validSlot, responseMessage.Slot);
            Assert.Equal(feed, responseMessage.Feed);
        }

        [Theory, AutoData]
        public void Constructor_GivenInvalidSlot_ShouldThrowException(Feed feed)
        {
            const byte invalidSlot = 60;

            Assert.Throws<SlotOutOfRangeException>(() => new RegisterFeedResponseMessage(invalidSlot, feed));
        }

        [Fact]
        public void Constructor_GivenNullFeed_ShouldThrowExcpetion()
        {
            const byte validSlot = 1;

            Assert.Throws<ArgumentNullException>(() => new RegisterFeedResponseMessage(validSlot, null!));
        }
    }
}
