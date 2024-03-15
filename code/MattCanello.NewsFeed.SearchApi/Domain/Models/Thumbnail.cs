﻿using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Models
{
    [Serializable]
    public sealed class Thumbnail
    {
        [Required]
        [DataType(DataType.ImageUrl)]
        public string? Url { get; set; }

        [Range(0, int.MaxValue)]
        public int? Width { get; set; }

        public string? Credit { get; set; }
    }
}
