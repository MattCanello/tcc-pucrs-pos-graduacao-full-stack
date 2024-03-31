using MattCanello.NewsFeed.Frontend.Server.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Models.Admin;
using System.Text.Json;

namespace MattCanello.NewsFeed.Frontend.Server.Clients
{
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

        public async Task<AdminQueryResponse<AdminFeed>> QueryFeedsAsync(AdminQueryCommand queryCommand, CancellationToken cancellationToken = default)
            => await QueryAsync<AdminFeed>("/feed", queryCommand, cancellationToken);

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

        public async Task<AdminChannel> GetChannelAsync(string channelId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channelId);

            return await SimpleGetAsync<AdminChannel>($"/channel/{channelId}", cancellationToken);
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
