using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.RssReader.Domain.Models
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

        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Url)]
        public string? URL { get; set; }
    }
}
