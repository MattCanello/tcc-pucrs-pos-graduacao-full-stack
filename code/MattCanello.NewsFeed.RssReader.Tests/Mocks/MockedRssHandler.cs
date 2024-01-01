using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;

namespace MattCanello.NewsFeed.RssReader.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal sealed class MockedRssHandler : DelegatingHandler
    {
        private readonly string? _rss;
        private readonly string? _etag;
        private readonly DateTimeOffset? _lastPublishDate;

        public MockedRssHandler(string? rss = null, string? etag = null, DateTimeOffset? lastPublishDate = null)
        {
            _rss = rss;
            _etag = etag;
            _lastPublishDate = lastPublishDate;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpContent? content = null;

            if (!string.IsNullOrEmpty(_rss))
                content = new StringContent(_rss, Encoding.UTF8, "text/xml");

            var statusCode = content is null ? HttpStatusCode.NoContent : HttpStatusCode.OK;

            var isDateUpdated = request.Headers.IfModifiedSince != null && _lastPublishDate <= request.Headers.IfModifiedSince;
            var isETagUpdated = request.Headers.IfNoneMatch != null && request.Headers.IfNoneMatch.Any() && _etag == request.Headers.IfNoneMatch.First().ToString();

            if (isDateUpdated || isETagUpdated)
                statusCode = HttpStatusCode.NotModified;

            var response = new HttpResponseMessage(statusCode)
            {
                Content = content
            };

            if (content is null)
                response.Content = null;

            if (!string.IsNullOrEmpty(_etag))
                response.Headers.Add("etag", _etag);

            if (_lastPublishDate.HasValue)
                response.Headers.Date = _lastPublishDate.Value;

            return Task.FromResult(response);
        }
    }
}
