namespace MattCanello.NewsFeed.Frontend.Server.Infrastructure.Models.Admin
{
    public sealed record AdminQueryCommand
    {
        public AdminQueryCommand() { }

        public AdminQueryCommand(int pageSize, int? skip = 0)
        {
            PageSize = pageSize;
            Skip = skip;
        }

        public int? PageSize { get; init; }
        public int? Skip { get; init; }
    }
}
