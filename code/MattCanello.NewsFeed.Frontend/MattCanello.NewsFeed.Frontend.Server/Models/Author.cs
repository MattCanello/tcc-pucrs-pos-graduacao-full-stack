using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.Frontend.Server.Models
{
    [Serializable]
    public sealed record Author
    {
        public Author() { }

        public Author(string name, string? email = null)
        {
            Name = name;
            Email = email;
        }

        [Required]
        public string? Name { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }
}
