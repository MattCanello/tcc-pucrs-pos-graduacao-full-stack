﻿using MattCanello.NewsFeed.RssReader.Domain.Models;

namespace MattCanello.NewsFeed.RssReader.Domain.Interfaces.EventPublishers
{
    public interface INewEntryFoundPublisher
    {
        Task PublishAsync(string feedId, Entry entry, CancellationToken cancellationToken = default);
    }
}
