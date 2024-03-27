namespace MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Models
{
    [Serializable]
    public sealed class ChannelElasticModel
    {
        public string? ChannelId { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        // TODO: Remover Language de 
        public string? Language { get; set; }

        public string? Copyright { get; set; }

        public string? ImageUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
