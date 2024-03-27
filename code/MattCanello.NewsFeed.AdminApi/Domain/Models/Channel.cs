﻿using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Domain.Models
{
    [Serializable]
    public sealed record Channel
    {
        [Required]
        [StringLength(100)]
        public string ChannelId { get; set; }

        public string? Name { get; set; }

        [DataType(DataType.Url)]
        public string? Url { get; set; }

        // TODO: Remover Language de Channel
        public string? Language { get; set; }

        public string? Copyright { get; set; }

        [DataType(DataType.ImageUrl)]
        public string? ImageUrl { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
