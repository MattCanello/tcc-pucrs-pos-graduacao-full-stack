﻿using AutoMapper;
using MattCanello.NewsFeed.RssReader.Domain.Profiles;

namespace MattCanello.NewsFeed.RssReader.Tests
{
    internal static class Util
    {
        public static readonly IMapper Mapper = new MapperConfiguration(config =>
        {
            config.AddProfile<FeedProfile>();
        }).CreateMapper();
    }
}
