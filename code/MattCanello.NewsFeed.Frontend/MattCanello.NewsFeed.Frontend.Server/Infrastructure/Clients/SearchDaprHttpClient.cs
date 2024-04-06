using Dapr.Client;
using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Search;
using System.Net;
using System.Text.Json;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Clients
{
    public sealed class SearchDaprHttpClient : ISearchClient
    {
        private readonly DaprClient _daprClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        const string SearchApiAppId = "search-api";

        public SearchDaprHttpClient(DaprClient daprClient, JsonSerializerOptions jsonSerializerOptions)
        {
            _daprClient = daprClient;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<SearchResponse<SearchDocument>> GetRecentAsync(string? channelId = null, int? size = null, CancellationToken cancellationToken = default)
        {
            var url = string.IsNullOrEmpty(channelId)
                ? $"/feed/recent?size={size}"
                : $"/feed/{channelId}/recent?size={size}";

            using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, SearchApiAppId, url);
            using var response = await _daprClient.InvokeMethodWithResponseAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            if (response.Content is null || response.StatusCode == HttpStatusCode.NoContent)
                return SearchResponse<SearchDocument>.CreateEmpty(size, skip: null);

            return await response.Content.ReadFromJsonAsync<SearchResponse<SearchDocument>>(_jsonSerializerOptions, cancellationToken)
                ?? SearchResponse<SearchDocument>.CreateEmpty(size, skip: null);
        }

        public async Task<SearchResponse<SearchDocument>> SearchAsync(SearchCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var url = $"/search?q={command.Query}&size={command.PageSize}&skip={command.Skip}&feedId={command.FeedId}";

            using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, SearchApiAppId, url);
            using var response = await _daprClient.InvokeMethodWithResponseAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            if (response.Content is null || response.StatusCode == HttpStatusCode.NoContent)
                return SearchResponse<SearchDocument>.CreateEmpty(command);

            return await response.Content.ReadFromJsonAsync<SearchResponse<SearchDocument>>(_jsonSerializerOptions, cancellationToken)
                ?? SearchResponse<SearchDocument>.CreateEmpty(command);
        }

        public async Task<SearchDocument?> GetDocumentByIdAsync(string feedId, string id, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(id);

            var url = $"/feed/{feedId}/document/{id}";

            using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, SearchApiAppId, url);
            using var response = await _daprClient.InvokeMethodWithResponseAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<SearchDocument?>(_jsonSerializerOptions, cancellationToken);
        }
    }
}
