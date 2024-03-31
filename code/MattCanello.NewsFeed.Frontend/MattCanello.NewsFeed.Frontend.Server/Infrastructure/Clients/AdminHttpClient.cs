using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin;
using System.Text.Json;

namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Clients
{
    // TODO: Trocar para usar o Dapr - https://docs.dapr.io/developing-applications/building-blocks/service-invocation/howto-invoke-discover-services/
    public sealed class AdminHttpClient : IAdminClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AdminHttpClient(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
        {
            _httpClient = httpClient;
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

            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            response.EnsureSuccessStatusCode();

            if (response.Content is null)
                return AdminQueryResponse<TModel>.Empty;

            return await response.Content
                .ReadFromJsonAsync<AdminQueryResponse<TModel>>(_jsonSerializerOptions, cancellationToken)
                ?? AdminQueryResponse<TModel>.Empty;
        }

        public async Task<AdminFeedWithChannel> GetFeedAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            return await SimpleGetAsync<AdminFeedWithChannel>($"/feed/{feedId}", cancellationToken);
        }

        private async Task<TModel> SimpleGetAsync<TModel>(string path, CancellationToken cancellationToken = default)
            where TModel : class, new()
        {
            ArgumentNullException.ThrowIfNull(path);

            using var response = await _httpClient.GetAsync(path, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            var model = await response.Content.ReadFromJsonAsync<TModel>(_jsonSerializerOptions, cancellationToken);

            return model!;
        }
    }
}
