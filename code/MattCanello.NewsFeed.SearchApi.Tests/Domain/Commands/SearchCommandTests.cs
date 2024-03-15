﻿using AutoFixture.Xunit2;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Models;

namespace MattCanello.NewsFeed.SearchApi.Tests.Domain.Commands
{
    public sealed class SearchCommandTests
    {
        [Fact]
        public void Constructor_GivenNoParams_ShouldCreateInstanceWithDefaultExpectedValues()
        {
            var command = new SearchCommand();

            Assert.Null(command.Query);
            Assert.Null(command.Paging);
            Assert.Null(command.FeedId);
        }

        [Theory, AutoData]
        public void Constructor_GivenSpecificParams_ShouldPreserveGivenData(string query, Paging paging, string feedId)
        {
            var command = new SearchCommand(query, paging, feedId);

            Assert.Equal(query, command.Query);
            Assert.Equal(paging, command.Paging);
            Assert.Equal(feedId, command.FeedId);
        }
    }
}
