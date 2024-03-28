using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Domain.Responses;
using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Models;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Repositories
{
    public sealed class ElasticFeedRepository : IFeedRepository
    {
        const string FeedsIndexName = "feeds";
        private readonly IElasticSearchRepository _elasticSearchRepository;
        private readonly IElasticSearchManagementRepository _elasticSearchManagementRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IMapper _mapper;

        public ElasticFeedRepository(
            IElasticSearchRepository elasticSearchRepository,
            IElasticSearchManagementRepository elasticSearchManagementRepository,
            IChannelRepository channelRepository,
            IMapper mapper)
        {
            _elasticSearchRepository = elasticSearchRepository;
            _elasticSearchManagementRepository = elasticSearchManagementRepository;
            _channelRepository = channelRepository;
            _mapper = mapper;
        }

        public async Task<FeedWithChannel> CreateAsync(FeedWithChannel feed, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feed);

            var elasticModel = _mapper.Map<FeedElasticModel>(feed);

            await EnsureIndexExistsAsync(cancellationToken);

            await _elasticSearchRepository.IndexAsync(feed.FeedId, elasticModel, FeedsIndexName, cancellationToken);

            return feed;
        }

        public async Task<FeedWithChannel?> GetByIdAsync(string feedId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feedId);

            var elasticModel = await _elasticSearchRepository.GetByIdAsync<FeedElasticModel>(feedId, FeedsIndexName, cancellationToken);

            var feed = _mapper.Map<FeedWithChannel>(elasticModel);

            if (feed != null && !string.IsNullOrEmpty(elasticModel?.ChannelId))
                feed.Channel = await _channelRepository.GetByIdAsync(elasticModel.ChannelId, cancellationToken);

            return feed;
        }

        public async Task<FeedWithChannel> UpdateAsync(FeedWithChannel feed, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(feed);

            var elasticModel = _mapper.Map<FeedElasticModel>(feed);

            await EnsureIndexExistsAsync(cancellationToken);

            await _elasticSearchRepository.IndexAsync(feed.FeedId, elasticModel, FeedsIndexName, cancellationToken);

            return feed;
        }

        public async Task<QueryResponse<Feed>> QueryAsync(QueryCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var result = await _elasticSearchRepository.QueryAsync<FeedElasticModel>(
                command.PageSize ?? QueryCommand.DefaultPageSize, 
                command.Skip ?? 0, 
                FeedsIndexName,
                sort => sort.Ascending(t => t.FeedId),
                cancellationToken);

            return new QueryResponse<Feed>(
                result.Total,
                result.Items.Select(item => _mapper.Map<Feed>(item)));
        }

        private async Task EnsureIndexExistsAsync(CancellationToken cancellationToken = default)
        {
            await _elasticSearchManagementRepository.EnsureIndexExistsAsync(FeedsIndexName, idx => idx.Map(map => map
                .Properties<FeedElasticModel>(p => p
                    .Keyword(field => field.Name("feedId"))
                    .Keyword(field => field.Name("channelId"))
                    .Text(field => field.Name("name"))
                    .Keyword(field => field.Name("language"))
                    .Text(field => field.Name("copyright"))
                    .Text(field => field.Name("imageUrl"))
                    .Text(field => field.Name("url"))
                    .DateNanos(field => field.Name("createdAt").Format("strict_date_optional_time_nanos"))
                )), cancellationToken);
        }
    }
}
