using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Domain.Models
{
    [Serializable]
    public sealed class Author
    {
        public Author(string name, string? email = null, string? url = null)
        {
            Name = name;
            Email = email;
            URL = url;
        }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Url)]
        public string? URL { get; set; }
    }
}
