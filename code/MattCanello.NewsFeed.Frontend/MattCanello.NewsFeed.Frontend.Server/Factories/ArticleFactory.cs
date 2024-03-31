using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Models;
using MattCanello.NewsFeed.Frontend.Server.Models.Search;

namespace MattCanello.NewsFeed.Frontend.Server.Factories
{
    public sealed class ArticleFactory : IArticleFactory
    {
        private readonly IMapper _mapper;
        private readonly IArticleDetailsFactory _articleDetailsFactory;

        public ArticleFactory(IMapper mapper, IArticleDetailsFactory articleDetailsFactory)
        {
            _mapper = mapper;
            _articleDetailsFactory = articleDetailsFactory;
        }

        public Article FromSearch(SearchDocument document, Channel channel, Feed feed)
        {
            ArgumentNullException.ThrowIfNull(document);

            return new Article
            {
                Id = document.Id,
                Channel = channel,
                Feed = feed,
                PublishDate = document.Entry.PublishDate,
                Title = document.Entry.Title,
                Authors = CreateAuthors(document.Entry.Authors),
                Thumbnail = SelectThumbnail(document.Entry.Thumbnails),
                Details = _articleDetailsFactory.FromDescription(document.Entry.Description)
            };
        }

        public ISet<Author> CreateAuthors(IEnumerable<SearchAuthor> authors)
        {
            ArgumentNullException.ThrowIfNull(authors);

            return new HashSet<Author>(authors.Select(aut => _mapper.Map<Author>(aut)));
        }

        public Thumbnail SelectThumbnail(IEnumerable<SearchThumbnail> searchThumbnails)
        {
            ArgumentNullException.ThrowIfNull(searchThumbnails);

            var selectedThumb = searchThumbnails
                .OrderByDescending(thumb => thumb.Width ?? 0)
                .FirstOrDefault();

            return _mapper.Map<Thumbnail>(selectedThumb);
        }
    }
}
