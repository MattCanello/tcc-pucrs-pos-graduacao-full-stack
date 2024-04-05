using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.AdminApi.Domain.Commands
{
    [Serializable]
    public sealed class CreateFeedCommand : IValidatableObject
    {
        [Required]
        [StringLength(100)]
        public string? FeedId { get; set; }

        [Required]
        [StringLength(100)]
        public string? ChannelId { get; set; }

        [Url]
        [Required]
        [DataType(DataType.Url)]
        public string? Url { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(FeedId) || string.IsNullOrWhiteSpace(ChannelId))
                yield break;

            if (!FeedId.StartsWith(ChannelId))
                yield return new ValidationResult("FeedId must start with the value provided for ChannelId", new string[1] { nameof(FeedId) });
        }
    }
}
