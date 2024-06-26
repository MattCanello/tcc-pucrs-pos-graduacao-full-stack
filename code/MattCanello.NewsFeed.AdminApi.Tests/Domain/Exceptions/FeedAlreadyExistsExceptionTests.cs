﻿using AutoFixture.Xunit2;
using MattCanello.NewsFeed.AdminApi.Domain.Exceptions;

namespace MattCanello.NewsFeed.AdminApi.Tests.Domain.Exceptions
{
    public class FeedAlreadyExistsExceptionTests
    {
        [Theory, AutoData]
        public void Constructor_GivenFeedId_ShouldPreserveFeedId(string feedId)
        {
            var ex = new FeedAlreadyExistsException(feedId);

            Assert.Equal(feedId, ex.FeedId);
            Assert.Equal("The feed already exists", ex.Message);
        }
    }
}
