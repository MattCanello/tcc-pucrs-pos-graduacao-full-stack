using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Domain.Commands
{
    [Serializable]
    public sealed record QueryCommand
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;

        [Range(1, MaxPageSize)]
        [FromQuery(Name = "pageSize")]
        [DefaultValue(DefaultPageSize)]
        public int? PageSize { get; init; } = DefaultPageSize;

        [DefaultValue(0)]
        [Range(0, int.MaxValue)]
        [FromQuery(Name = "skip")]
        public int? Skip { get; init; } = 0;
    }
}
