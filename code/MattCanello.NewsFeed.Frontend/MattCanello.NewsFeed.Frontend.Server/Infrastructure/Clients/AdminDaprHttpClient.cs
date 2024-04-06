using Dapr.Client;
using MattCanello.NewsFeed.Frontend.Server.Domain.Exceptions;
using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin;
using System.Net;
using System.Text.Json;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Clients
{
    public sealed class AdminDaprHttpClient : IAdminClient
    {
        private readonly DaprClient _daprClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        const string AdminApiAppId = "admin-api";

        public AdminDaprHttpClient(DaprClient daprClient, JsonSerializerOptions jsonSerializerOptions)
        {
            _daprClient = daprClient;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<AdminQueryResponse<AdminChannel>> QueryChannelsAsync(AdminQueryCommand queryCommand, CancellationToken cancellationToken = default)
            => await QueryAsync<AdminChannel>("/channel", queryCommand, cancellationToken);

        private async Task<AdminQueryResponse<TModel>> QueryAsync<TModel>(string baseUrl, AdminQueryCommand queryCommand, CancellationToken cancellationToken = default)
            where TModel : class, new()
        {
            ArgumentNullException.ThrowIfNull(queryCommand);
            ArgumentNullException.ThrowIfNull(baseUrl);

            var url = $"{baseUrl}?pageSize={queryCommand.PageSize}&skip={queryCommand.Skip}";

            using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, AdminApiAppId, url);
            using var response = await _daprClient.InvokeMethodWithResponseAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            if (response.Content is null || response.StatusCode == HttpStatusCode.NoContent)
                return AdminQueryResponse<TModel>.Empty;

            return await response.Content
                .ReadFromJsonAsync<AdminQueryResponse<TModel>>(_jsonSerializerOptions, cancellationToken)
                ?? AdminQueryResponse<TModel>.Empty;
        }

        public async Task<AdminFeedWithChannel> GetFeedAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var path = $"/feed/{feedId}";

            using var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, AdminApiAppId, path);
            using var response = await _daprClient.InvokeMethodWithResponseAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new FeedNotFoundException(feedId);

            response.EnsureSuccessStatusCode();

            var model = await response.Content.ReadFromJsonAsync<AdminFeedWithChannel>(_jsonSerializerOptions, cancellationToken);

            return model!;
        }
    }
}
