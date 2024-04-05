using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Models
{
    [Serializable]
    public sealed record Author
    {
        [Required]
        public string? Name { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }
}
