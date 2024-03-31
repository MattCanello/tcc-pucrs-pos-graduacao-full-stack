using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search;
using System.Net;
using System.Text.Json;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Clients
{
    // TODO: Trocar para usar o Dapr - https://docs.dapr.io/developing-applications/building-blocks/service-invocation/howto-invoke-discover-services/
    public sealed class SearchHttpClient : ISearchClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public SearchHttpClient(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<SearchRecentResponse> GetRecentAsync(string? feedId = null, int? size = null, CancellationToken cancellationToken = default)
        {
            var url = string.IsNullOrEmpty(feedId)
                ? $"/feed/recent?size={size}"
                : $"/feed/{feedId}/recent?size={size}";

            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            if (response.Content is null)
                return SearchRecentResponse.Empty;

            return await response.Content.ReadFromJsonAsync<SearchRecentResponse>(_jsonSerializerOptions, cancellationToken)
                ?? SearchRecentResponse.Empty;
        }

        public async Task<SearchResponse<SearchDocument>> SearchAsync(SearchCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var url = $"/search?q={command.Query}&size={command.PageSize}&skip={command.Skip}&feedId={command.FeedId}";

            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            if (response.Content is null)
                return SearchResponse<SearchDocument>.CreateEmpty(command);

            return await response.Content.ReadFromJsonAsync<SearchResponse<SearchDocument>>(_jsonSerializerOptions, cancellationToken)
                ?? SearchResponse<SearchDocument>.CreateEmpty(command);
        }

        public async Task<SearchDocument?> GetDocumentByIdAsync(string feedId, string id, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(id);

            var url = $"/feed/{feedId}/document/{id}";

            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<SearchDocument?>(_jsonSerializerOptions, cancellationToken);
        }
    }
}
