using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Domain.Commands
{
    [Serializable]
    public sealed class RssData
    {
        public string? Name { get; set; }

        [Url]
        [DataType(DataType.Url)]
        public string? Url { get; set; }
        public string? Language { get; set; }
        public string? Copyright { get; set; }

        [Url]
        [DataType(DataType.Url)]
        public string? ImageUrl { get; set; }
    }
}
