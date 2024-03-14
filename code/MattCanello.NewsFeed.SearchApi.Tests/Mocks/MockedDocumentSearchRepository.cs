using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
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

        public Key Add(Entry entry, string feedId)
        {
            ArgumentNullException.ThrowIfNull(entry);

            var key = new Key(feedId, entry.Id!);
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
            throw new NotImplementedException();
        }

        [Serializable]
        public record class Key(string FeedId, string EntryId)
        {
            public string IndexName => FeedId;
            public string Id => GetHashCode().ToString("F0");
        }
    }
}
