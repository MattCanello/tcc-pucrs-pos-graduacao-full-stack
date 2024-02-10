using AutoFixture.Xunit2;
using CloudNative.CloudEvents;
using MattCanello.NewsFeed.Cross.CloudEvents.Extensions;

namespace MattCanello.NewsFeed.Cross.CloudEvents.Tests.UnitTests.Extensions
{
    public class CloudEventExtensionsTests
    {
        [Fact]
        public void GetFeedId_GivenANullCloudEvent_ShouldReturnNull()
        {
            CloudEvent? cloudEvent = null;

            var feedId = cloudEvent!.GetFeedId();

            Assert.Null(feedId);
        }

        [Fact]
        public void GetFeedId_GivenACloudEventThatDontHaveFeedIdAttribute_ShouldReturnNull()
        {
            var cloudEvent = new CloudEvent();

            var feedId = cloudEvent.GetFeedId();

            Assert.Null(feedId);
        }

        [Theory, AutoData]
        public void GeedId_GivenAPopulatedFeedIdAttribute_ShouldReturnTheFeedId(string providedFeedId)
        {
            var cloudEvent = new CloudEvent();

            cloudEvent.SetAttributeFromString("feedid", providedFeedId);

            var feedId = cloudEvent.GetFeedId();

            Assert.Equal(providedFeedId, feedId);
        }

        [Theory, AutoData]
        public void SetFeedId_GivenANullEvent_ShouldNotThrowException(string feedId)
        {
            CloudEvent? cloudEvent = null;

            cloudEvent!.SetFeedId(feedId);
        }

        [Theory, AutoData]
        public void SetFeedId_GivenAValidInput_ShouldSetFeedId(string feedId)
        {
            var cloudEvent = new CloudEvent();

            cloudEvent.SetFeedId(feedId);

            var readFeedId = cloudEvent.GetFeedId();

            Assert.Equal(feedId, readFeedId);
        }
    }
}
