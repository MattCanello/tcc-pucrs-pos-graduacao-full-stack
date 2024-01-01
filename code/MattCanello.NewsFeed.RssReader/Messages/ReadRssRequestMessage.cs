namespace MattCanello.NewsFeed.RssReader.Messages
{
    [Serializable]
    public sealed class ReadRssRequestMessage
    {
        public ReadRssRequestMessage() { }

        public ReadRssRequestMessage(Uri uri, string? etag = null, DateTimeOffset? lastModifiedDate = null)
        {
            ArgumentNullException.ThrowIfNull(uri);

            Uri = uri;
            ETag = etag;
            LastModifiedDate = lastModifiedDate;
        }

        public Uri? Uri { get; set; }
        public string? ETag { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
