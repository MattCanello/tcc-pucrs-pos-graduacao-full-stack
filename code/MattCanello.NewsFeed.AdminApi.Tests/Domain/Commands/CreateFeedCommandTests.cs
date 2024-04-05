using AutoFixture.Xunit2;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Tests.Domain.Commands
{
    public class CreateFeedCommandTests
    {
        [Fact]
        public void Validate_GivenEmptyInstance_ShouldNotReturnErrors()
        {
            var command = new CreateFeedCommand();

            var validationErrors = command.Validate(new ValidationContext(command));

            Assert.NotNull(validationErrors);
            Assert.Empty(validationErrors);
        }

        [Theory, AutoData]
        public void Validate_GivenOnlyFeedId_ShouldNotReturnError(string feedId)
        {
            var command = new CreateFeedCommand() { FeedId = feedId };

            var validationErrors = command.Validate(new ValidationContext(command));

            Assert.NotNull(validationErrors);
            Assert.Empty(validationErrors);
        }

        [Theory, AutoData]
        public void Validate_GivenOnlyChannelId_ShouldNotReturnError(string channelId)
        {
            var command = new CreateFeedCommand() { ChannelId = channelId };

            var validationErrors = command.Validate(new ValidationContext(command));

            Assert.NotNull(validationErrors);
            Assert.Empty(validationErrors);
        }

        [Theory, AutoData]
        public void Validate_GivenFeedIdNotMatchingChannelId_ShouldReturnError(string feedId, string channelId)
        {
            var command = new CreateFeedCommand() { FeedId = feedId, ChannelId = channelId };

            var validationErrors = command.Validate(new ValidationContext(command));

            Assert.NotNull(validationErrors);
            Assert.NotEmpty(validationErrors);

            var validationError = Assert.Single(validationErrors);
            Assert.Equal("FeedId must start with the value provided for ChannelId", validationError.ErrorMessage);

            Assert.NotNull(validationError.MemberNames);
            Assert.NotEmpty(validationError.MemberNames);

            var memberName = Assert.Single(validationError.MemberNames);
            Assert.Equal("FeedId", memberName);
        }
    }
}
