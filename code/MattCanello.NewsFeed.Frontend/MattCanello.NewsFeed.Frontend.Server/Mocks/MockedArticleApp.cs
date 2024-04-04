using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Mocks
{
    sealed class MockedArticleApp : IArticleApp
    {
        private readonly Article _article1 = new Article()
        {
            Authors = new HashSet<Author>() { new Author("Emine Saner") },
            Channel = new Channel("the-guardian", "The Guardian"),
            Details = new ArticleDetails(
                "We chose Raymond, Row and Rodney as babies. They are gentel, curious personalities - who sometimes strut into the house in pursuit of spare rains...",
                "I loved alpacas when I was a child, but never did I think I would own three. I love their little faces and the fact that they are big, but so gentle. They are very shy and not very confident.",
                "We moved house last year and our new home has a bit of land. I had always wanted to have more animals; I read up on everything about alpacas for so long that it came to the point where I was like: “Right, I'm going to go for it. We could give them a wonderful home.” And we have."),
            Feed = new Feed("the-guardian-uk", "The Guardian UK"),
            Id = Guid.NewGuid().ToString(),
            PublishDate = new DateTimeOffset(2023, 12, 28, 14, 03, 00, new TimeSpan(-3, 0, 0)),
            Thumbnail = new Thumbnail("https://i.guim.co.uk/img/media/4b91e61aa1e89de5f2128166a6d837d4e07ac470/0_252_1440_864/master/1440.jpg?width=1300&dpr=2&s=none", "The Guardian", "'They're so fluffy!' (From left) Rodney, Roy and Raymond."),
            Title = "The pet I'll never forget: Mrs Hinch on her alpacas, 'who fill my gardin with happiness'"
        };

        private readonly Article _article2 = new Article()
        {
            Authors = new HashSet<Author>() { new Author("Anaísa Cattucci") },
            Channel = new Channel("g1", "G1"),
            Details = new ArticleDetails(
                "Próximo ano terá dez feriados nacionais, sendo que quatro serão no fim de semana.",
                "O ano de 2024 terá poucos feriados prolongados. Serão dez feriados nacionais, dos quais apenas três vão ser vizinhos de um fim de semana, isto é, caem numa segunda ou na sexta-feira.",
                "São eles: o da Confraternização Universal, em 1° de janeiro; o da Paixão de Cristo, em 29 de março; e o da Proclamação da República, no dia 15 de novembro (veja a lista com todos os feriados ao final da reportagem)."),
            Feed = new Feed("g1-brasil", "G1 Brasil"),
            Id = Guid.NewGuid().ToString(),
            PublishDate = new DateTimeOffset(2023, 11, 21, 04, 02, 00, new TimeSpan(-3, 0, 0)),
            Thumbnail = new Thumbnail("https://s2-g1.glbimg.com/wNL5K8Sa6WSocIECL0Q7D63WS5w=/1200x/smart/filters:cover():strip_icc()/s02.video.glbimg.com/x720/12131145.jpg", "G1", "'Feriadôes' de 2024: veja as folgas previstas"),
            Title = "2024 terá poucos 'feriadões' prolongados; veja as folgas previstas"
        };

        public Task<IEnumerable<Article>> GetFrontPageArticlesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<Article> articles = new List<Article>()
            {
                _article1,
                _article2
            };

            return Task.FromResult(articles);
        }

        public Task<Article?> GetArticleAsync(string feedId, string articleId, CancellationToken cancellationToken = default)
        {
            if (_article1.Feed!.FeedId == feedId && _article1.Id == articleId)
                return Task.FromResult<Article?>(_article1);

            if (_article2.Feed!.FeedId == feedId && _article2.Id == articleId)
                return Task.FromResult<Article?>(_article2);

            return Task.FromResult<Article?>(null);
        }

        public Task<IEnumerable<Article>> GetChannelArticlesAsync(string channelId, CancellationToken cancellationToken = default)
        {
            if (_article1.Channel!.ChannelId == channelId)
                return Task.FromResult<IEnumerable<Article>>(new Article[] { _article1 });

            if (_article2.Channel!.ChannelId == channelId)
                return Task.FromResult<IEnumerable<Article>>(new Article[] { _article2 });

            return Task.FromResult<IEnumerable<Article>>(Array.Empty<Article>());
        }

        public Task<IEnumerable<Article>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
