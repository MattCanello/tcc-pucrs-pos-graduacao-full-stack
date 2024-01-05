using AutoFixture.Xunit2;
using FluentAssertions;
using MattCanello.NewsFeed.RssReader.Controllers;
using MattCanello.NewsFeed.RssReader.Domain.Commands;
using MattCanello.NewsFeed.RssReader.Domain.Models;
using MattCanello.NewsFeed.RssReader.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace MattCanello.NewsFeed.RssReader.Tests.IntegrationTests
{
    public sealed class FeedControllerTests
    {
        [Theory(DisplayName = "Cadastro de Feed válido"), AutoData]
        public async Task Create_WhenDataIsValid_ShouldCreateFeed(CreateFeedCommand createFeedMessage)
        {
            var controller = new FeedController(new InMemoryFeedRepository(), Util.Mapper);

            var result = await controller.Create(createFeedMessage);

            var createdResult = result.Should()
                .NotBeNull()
                .And
                .BeOfType<CreatedAtActionResult>()
                .Subject;

            var feed = createdResult.Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType<Feed>()
                .Subject;

            feed.FeedId.Should().Be(createFeedMessage.FeedId);
            feed.Url.Should().Be(createFeedMessage.Url);

            feed.LastETag.Should().BeNull();
            feed.LastModifiedDate.Should().BeNull();
        }

        [Fact(DisplayName = "Cadastro de Feed null")]
        public async Task Create_WhenMessageIsNull_ShouldReturnBadRequest()
        {
            var controller = new FeedController(new InMemoryFeedRepository(), Util.Mapper);

            var result = await controller.Create(null!);

            result.Should()
                .NotBeNull()
                .And
                .BeOfType<BadRequestResult>();
        }

        [Theory(DisplayName = "Obtenção de Feed não existente"), AutoData]
        public async Task Get_WhenFeedDoesNotExist_ShouldReturnNotFound(string feedId)
        {
            var controller = new FeedController(new InMemoryFeedRepository(), Util.Mapper);

            var result = await controller.Get(feedId);

            result.Should()
                .NotBeNull()
                .And
                .BeOfType<NotFoundResult>();
        }

        [Theory(DisplayName = "Obtenção de Feed existente"), AutoData]
        public async Task Get_WhenFeedExists_ShouldReturnExpectedFeed(Feed feed)
        {
            var controller = new FeedController(new InMemoryFeedRepository(feed), Util.Mapper);

            var result = await controller.Get(feed.FeedId);

            result.Should()
                .NotBeNull()
                .And
                .BeOfType<OkObjectResult>()
                .Which
                .Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType<Feed>()
                .Which
                .Should()
                .BeEquivalentTo(feed);
        }

        [Theory(DisplayName = "Exclusão de Feed existente"), AutoData]
        public async Task Delete_WhenFeedExists_ShouldReturnNoContent(Feed feed)
        {
            var controller = new FeedController(new InMemoryFeedRepository(feed), Util.Mapper);

            var result = await controller.Delete(feed.FeedId);

            result.Should()
                .NotBeNull()
                .And
                .BeOfType<NoContentResult>();
        }

        [Theory(DisplayName = "Exclusão de Feed inexistente"), AutoData]
        public async Task Delete_WhenFeedDoesNotExist_ShouldReturnNoContent(string feedId)
        {
            var controller = new FeedController(new InMemoryFeedRepository(), Util.Mapper);

            var result = await controller.Delete(feedId);

            result.Should()
                .NotBeNull()
                .And
                .BeOfType<NoContentResult>();
        }
    }
}
