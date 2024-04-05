using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Tests.Mocks
{
    sealed class MockedFrontPageConfiguration : IFrontPageConfiguration
    {
        private readonly int _frontPageNumberOfArticles;

        public MockedFrontPageConfiguration(int frontPageNumberOfArticles) 
            => _frontPageNumberOfArticles = frontPageNumberOfArticles;

        public int FrontPageNumberOfArticles() 
            => _frontPageNumberOfArticles;
    }
}
