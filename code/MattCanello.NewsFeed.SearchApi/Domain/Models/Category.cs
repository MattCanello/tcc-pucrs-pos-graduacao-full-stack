using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Models
{
    [Serializable]
    public sealed class Category
    {
        public Category() : this(string.Empty) { }

        public Category(string categoryName) 
            => CategoryName = categoryName;

        [Required]
        public string CategoryName { get; set; }
    }
}