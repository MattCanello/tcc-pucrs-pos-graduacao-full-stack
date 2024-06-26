﻿using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Domain.Models
{
    [Serializable]
    public sealed class FeedWithChannel
    {
        [Required]
        [StringLength(100)]
        public string FeedId { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string Url { get; set; }

        public Channel? Channel { get; set; }

        public string? Name { get; set; }

        public string? Language { get; set; }

        public string? Copyright { get; set; }

        [DataType(DataType.ImageUrl)]
        public string? ImageUrl { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
