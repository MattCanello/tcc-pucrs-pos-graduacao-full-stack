﻿using MattCanello.NewsFeed.Frontend.Server.Domain.Interfaces;
using MattCanello.NewsFeed.Frontend.Server.Domain.Models;

namespace MattCanello.NewsFeed.Frontend.Server.Domain.Application
{
    public sealed class ArticleApp : IArticleApp
    {
        private readonly ISearchClient _searchClient;
        private readonly IFeedRepository _feedRepository;
        private readonly IArticleFactory _articleFactory;
        private readonly IFrontPageConfiguration _frontPageConfiguration;

        public ArticleApp(ISearchClient searchClient, IFeedRepository feedRepository, IArticleFactory articleFactory, IFrontPageConfiguration frontPageConfiguration)
        {
            _searchClient = searchClient;
            _feedRepository = feedRepository;
            _articleFactory = articleFactory;
            _frontPageConfiguration = frontPageConfiguration;
        }

        public async Task<IEnumerable<Article>> GetFrontPageArticlesAsync(CancellationToken cancellationToken = default)
        {
            var numberOfArticles = _frontPageConfiguration.FrontPageNumberOfArticles();
            var response = await _searchClient.GetRecentAsync(size: numberOfArticles, cancellationToken: cancellationToken);
            var articles = new List<Article>(capacity: response.Results.Count);

            foreach (var document in response.Results)
            {
                var (feed, channel) = await _feedRepository.GetFeedAndChannelAsync(document.FeedId, cancellationToken);

                var article = _articleFactory.FromSearch(document, channel, feed);
                articles.Add(article);
            }

            return articles;
        }

        public async Task<Article?> GetArticleAsync(string feedId, string articleId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);
            ArgumentNullException.ThrowIfNull(articleId);

            var getDocumentTask = _searchClient.GetDocumentByIdAsync(feedId, articleId, cancellationToken);
            var getFeedAndChannelTask = _feedRepository.GetFeedAndChannelAsync(feedId, cancellationToken);

            await Task.WhenAll(getDocumentTask, getFeedAndChannelTask);

            var document = getDocumentTask.Result;

            if (document is null)
                return null;

            var (feed, channel) = getFeedAndChannelTask.Result;

            var article = _articleFactory.FromSearch(document, channel, feed);

            return article;
        }
    }
}