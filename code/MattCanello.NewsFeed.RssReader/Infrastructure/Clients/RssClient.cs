using MattCanello.NewsFeed.RssReader.Domain.Interfaces.Clients;
using MattCanello.NewsFeed.RssReader.Domain.Messages;
using MattCanello.NewsFeed.RssReader.Infrastructure.Interfaces.Evaluators;
using System.Net;
using System.Net.Http.Headers;
using System.Xml;

namespace MattCanello.NewsFeed.RssReader.Infrastructure.Clients
{
    public sealed class RssClient : IRssClient
    {
        private readonly HttpClient _httpClient;
        private readonly ISyndicationFeedEvaluator _syndicationFeedEvaluator;

        private static readonly IReadOnlyList<MediaTypeWithQualityHeaderValue> DefaultAcceptHeaders = new List<MediaTypeWithQualityHeaderValue>(5)
        {
            MediaTypeWithQualityHeaderValue.Parse("application/rss+xml"),
            MediaTypeWithQualityHeaderValue.Parse("application/rdf+xml"),
            MediaTypeWithQualityHeaderValue.Parse("application/atom+xml"),
            MediaTypeWithQualityHeaderValue.Parse("application/xml"),
            MediaTypeWithQualityHeaderValue.Parse("text/xml")
        };

        public RssClient(HttpClient httpClient, ISyndicationFeedEvaluator syndicationFeedEvaluator)
        {
            _httpClient = httpClient;
            _syndicationFeedEvaluator = syndicationFeedEvaluator;
        }

        public async Task<ReadRssResponseMessage> ReadAsync(ReadRssRequestMessage rssReaderRequest, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(rssReaderRequest);

            using var request = new HttpRequestMessage(HttpMethod.Get, rssReaderRequest.Uri);
            request.Version = new Version(2, 0);
            AddDefaultAcceptHeaders(request);
            SetupCachingHeaders(request, rssReaderRequest);

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (response.StatusCode == HttpStatusCode.NotModified)
                return ReadRssResponseMessage.NotModified;

            response.EnsureSuccessStatusCode();

            if (response.Content is null || response.StatusCode == HttpStatusCode.NoContent)
                return new ReadRssResponseMessage(response.Headers.ETag?.ToString(), response.Headers.Date);

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = XmlReader.Create(stream);

            var feed = _syndicationFeedEvaluator.DetermineLoaderStrategy(reader).Load(reader);
            return new ReadRssResponseMessage(feed, response.Headers.ETag?.ToString(), response.Headers.Date);
        }

        private static void AddDefaultAcceptHeaders(HttpRequestMessage request)
        {
            ArgumentNullException.ThrowIfNull(request);

            request.Headers.Accept.Clear();
            foreach (var header in DefaultAcceptHeaders)
                request.Headers.Accept.Add(header);
        }

        internal static void SetupCachingHeaders(HttpRequestMessage httpRequest, ReadRssRequestMessage rssReaderRequest)
        {
            ArgumentNullException.ThrowIfNull(httpRequest);
            ArgumentNullException.ThrowIfNull(rssReaderRequest);

            if (rssReaderRequest.LastModifiedDate != null)
                httpRequest.Headers.IfModifiedSince = rssReaderRequest.LastModifiedDate;

            if (!string.IsNullOrEmpty(rssReaderRequest.ETag))
                httpRequest.Headers.IfNoneMatch.ParseAdd(rssReaderRequest.ETag);
        }
    }
}
