namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces
{
    public interface IQueryStringProcessor
    {
        string Process(string? query);
    }
}