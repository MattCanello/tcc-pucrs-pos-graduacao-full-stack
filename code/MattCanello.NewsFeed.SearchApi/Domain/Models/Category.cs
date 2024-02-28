using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Models
{
    [Serializable]
    public record Category([Required] string CategoryName, string? Label = null, string? Schema = null);
}