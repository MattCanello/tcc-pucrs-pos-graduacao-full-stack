using MattCanello.NewsFeed.Frontend.Server.Interfaces;

namespace MattCanello.NewsFeed.Frontend.Server.Configuration
{
    sealed class FrontPageConfiguration : IFrontPageConfiguration
    {
        private readonly int _frontPgeNumberOfArticles;

        public FrontPageConfiguration(IConfiguration configuration)
        {
            if (!int.TryParse(configuration[EnvironmentVariables.FRONTPAGE_ARTICLE_COUNT], out _frontPgeNumberOfArticles))
                _frontPgeNumberOfArticles = 10;
        }

        public int FrontPageNumberOfArticles()
            => _frontPgeNumberOfArticles;
    }
}
