namespace MattCanello.NewsFeed.CronApi.Domain.Exceptions
{
    public sealed class SlotOutOfRangeException : ApplicationException
    {
        const string BaseMessage = "The provided slot number is out of range. A slot must be between 0 and 59.";

        public SlotOutOfRangeException()
            : base(BaseMessage) { }

        public SlotOutOfRangeException(byte providedSlot)
            : base($"{BaseMessage} The provided slot was '{providedSlot}'.")
        {
            ProvidedSlot = providedSlot;
        }

        public byte? ProvidedSlot { get; }

        public static void ThrowIfOutOfRange(byte slot)
        {
            if (slot >= 0 && slot <= 59)
                return;

            throw new SlotOutOfRangeException(slot);
        }
    }
}
