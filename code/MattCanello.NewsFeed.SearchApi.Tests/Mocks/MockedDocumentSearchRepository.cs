﻿using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using MattCanello.NewsFeed.SearchApi.Domain.Responses;
using DocumentSearchResponse = MattCanello.NewsFeed.SearchApi.Domain.Responses.SearchResponse<MattCanello.NewsFeed.SearchApi.Domain.Models.Document>;

namespace MattCanello.NewsFeed.SearchApi.Tests.Mocks
{
    internal sealed class MockedDocumentSearchRepository : IDocumentSearchRepository
    {
        private readonly IDictionary<Key, Entry> _data;

        public MockedDocumentSearchRepository(IDictionary<Key, Entry>? data = null)
        {
            _data = data ?? new Dictionary<Key, Entry>();
        }

        public MockedDocumentSearchRepository(params (string, Entry)[] feedIdAndEntries)
        {
            _data = new Dictionary<Key, Entry>();

            if (feedIdAndEntries is null)
                return;

            foreach (var (feedId, entry) in feedIdAndEntries)
                this.Add(entry, feedId);
        }

        public MockedDocumentSearchRepository(params (string, string, Entry)[] feedIdAndEntries)
        {
            _data = new Dictionary<Key, Entry>();
            
            if (feedIdAndEntries is null)
                return;

            foreach (var (channelId, feedId, entry) in feedIdAndEntries)
                this.Add(entry, channelId, feedId);
        }

        public Key Add(Entry entry, string feedId)
        {
            ArgumentNullException.ThrowIfNull(entry);

            var key = new Key(feedId, feedId, entry.Id!);
            _data[key] = entry;

            return key;
        }

        public Key Add(Entry entry, string channelId, string feedId)
        {
            ArgumentNullException.ThrowIfNull(entry);

            var key = new Key(channelId, feedId, entry.Id!);
            _data[key] = entry;

            return key;
        }

        public Task<FindResponse<Document>> FindByIdAsync(string entryId, string feedId, CancellationToken cancellationToken = default)
        {
            var item = _data
                .Where(kvp => kvp.Key.FeedId == feedId)
                .FirstOrDefault(kvp => kvp.Key.EntryId == entryId);

            if (item.Value is null)
                return Task.FromResult(FindResponse<Document>.NotFound);

            return Task.FromResult(new FindResponse<Document>(item.Key.Id, new Document(item.Key.Id, item.Key.FeedId, item.Value), item.Key.IndexName));
        }

        public Task<DocumentSearchResponse> SearchAsync(string? query = null, Paging? paging = null, string? feedId = null, CancellationToken cancellationToken = default)
        {
            IEnumerable<KeyValuePair<Key, Entry>> entries = _data;

            if (!string.IsNullOrEmpty(feedId))
                entries = _data.Where(kvp => kvp.Key.FeedId == feedId);

            if (!string.IsNullOrEmpty(query))
                entries = entries
                    .Where(e => (e.Value.Title ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase));

            int countPrePaging = entries.Count();

            if (paging != null)
                entries = entries.Skip(paging.Skip).Take(paging.Size);

            return Task.FromResult(new DocumentSearchResponse()
            {
                Paging = paging,
                Total = countPrePaging,
                Results = entries.Select(entries => new Document(entries.Key.Id, entries.Key.FeedId, entries.Value)).ToList()
            });
        }

        public Task<DocumentSearchResponse> GetRecentAsync(Paging paging, string? feedId = null, string? channelId = null, CancellationToken cancellationToken = default)
        {
            IEnumerable<KeyValuePair<Key, Entry>> entries = _data.OrderBy(kvp => kvp.Value.PublishDate);

            if (!string.IsNullOrEmpty(channelId))
                entries = _data.Where(kvp => kvp.Key.ChannelId == channelId);

            if (!string.IsNullOrEmpty(feedId))
                entries = _data.Where(kvp => kvp.Key.FeedId == feedId);

            int countPrePaging = entries.Count();

            if (paging != null)
                entries = entries.Skip(paging.Skip).Take(paging.Size);

            return Task.FromResult(new DocumentSearchResponse()
            {
                Paging = paging,
                Total = countPrePaging,
                Results = entries.Select(entries => new Document(entries.Key.Id, entries.Key.FeedId, entries.Value)).ToList()
            });
        }

        public Task<DocumentSearchResponse> SearchByChannelAsync(string? query = null, Paging? paging = null, string? channelId = null, CancellationToken cancellationToken = default)
        {
            IEnumerable<KeyValuePair<Key, Entry>> entries = _data.OrderBy(kvp => kvp.Value.PublishDate);

            if (!string.IsNullOrEmpty(channelId))
                entries = _data.Where(kvp => kvp.Key.ChannelId == channelId);

            int countPrePaging = entries.Count();

            if (paging != null)
                entries = entries.Skip(paging.Skip).Take(paging.Size);

            return Task.FromResult(new DocumentSearchResponse()
            {
                Paging = paging,
                Total = countPrePaging,
                Results = entries.Select(entries => new Document(entries.Key.Id, entries.Key.FeedId, entries.Value)).ToList()
            });
        }

        [Serializable]
        public record class Key(string ChannelId, string FeedId, string EntryId)
        {
            public string IndexName => $"{ChannelId}/{FeedId}";
            public string Id => GetHashCode().ToString("F0");
        }
    }
}
