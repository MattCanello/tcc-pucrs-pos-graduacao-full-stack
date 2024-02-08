using MattCanello.NewsFeed.CronApi.Domain.Exceptions;
using MattCanello.NewsFeed.CronApi.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.CronApi.Domain.Messages
{
    [Serializable]
    public record RegisterFeedResponseMessage
    {
        public RegisterFeedResponseMessage(byte slot, Feed feed)
        {
            SlotOutOfRangeException.ThrowIfOutOfRange(slot);
            ArgumentNullException.ThrowIfNull(feed);

            Slot = slot;
            Feed = feed;
        }

        [Required]
        [Range(0, 59)]
        public byte Slot { get; init; }

        [Required]
        public Feed Feed { get; init; }
    }
}
