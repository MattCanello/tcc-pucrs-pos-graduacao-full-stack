namespace MattCanello.NewsFeed.AdminApi.Domain.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        private const string DefaultMessage = "The requested resource was not found";

        public NotFoundException(string? message = null) : base(message ?? DefaultMessage)
        { }
    }
}
