using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Models
{
    [Serializable]
    public sealed class Category
    {
        public Category() : this(string.Empty) { }

        public Category(string categoryName, string? label = null, string? schema = null) 
        {
            CategoryName = categoryName;
            Label = label;
            Schema = schema;
        }

        [Required]
        public string CategoryName { get; set; }

        public string? Label { get; set; }

        public string? Schema { get; set; }
    }
}