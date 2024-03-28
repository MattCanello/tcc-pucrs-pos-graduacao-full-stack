using AutoMapper;
using MattCanello.NewsFeed.AdminApi.Domain.Commands;
using MattCanello.NewsFeed.AdminApi.Domain.Interfaces;
using MattCanello.NewsFeed.AdminApi.Domain.Models;
using MattCanello.NewsFeed.AdminApi.Domain.Responses;
using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Models;

namespace MattCanello.NewsFeed.AdminApi.Infrastructure.ElasticSearch.Repositories
{
    public sealed class ElasticChannelRepository : IChannelRepository
    {
        const string ChannelsIndexName = "channels";
        private readonly IElasticSearchRepository _elasticSearchRepository;
        private readonly IElasticSearchManagementRepository _elasticSearchManagementRepository;
        private readonly IMapper _mapper;

        public ElasticChannelRepository(IElasticSearchRepository elasticSearchRepository, IElasticSearchManagementRepository elasticSearchManagementRepository, IMapper mapper)
        {
            _elasticSearchRepository = elasticSearchRepository;
            _elasticSearchManagementRepository = elasticSearchManagementRepository;
            _mapper = mapper;
        }

        public async Task<Channel> CreateAsync(Channel channel, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channel);

            var elasticModel = _mapper.Map<ChannelElasticModel>(channel);

            await EnsureIndexExistsAsync(cancellationToken);

            await _elasticSearchRepository.IndexAsync(channel.ChannelId, elasticModel, ChannelsIndexName, cancellationToken);

            return channel;
        }

        public async Task<Channel> UpdateAsync(Channel channel, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channel);

            var elasticModel = _mapper.Map<ChannelElasticModel>(channel);

            await EnsureIndexExistsAsync(cancellationToken);

            await _elasticSearchRepository.IndexAsync(channel.ChannelId, elasticModel, ChannelsIndexName, cancellationToken);

            return channel;
        }

        public async Task<Channel?> GetByIdAsync(string channelId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(channelId);

            var elasticModel = await _elasticSearchRepository.GetByIdAsync<ChannelElasticModel>(channelId, ChannelsIndexName, cancellationToken);

            var channel = _mapper.Map<Channel>(elasticModel);

            return channel;
        }

        public async Task<QueryResponse<Channel>> QueryAsync(QueryCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var result = await _elasticSearchRepository.QueryAsync<ChannelElasticModel>(
                command.PageSize ?? QueryCommand.DefaultPageSize,
                command.Skip ?? 0,
                ChannelsIndexName,
                sort => sort.Ascending(t => t.ChannelId),
                cancellationToken);

            return new QueryResponse<Channel>(
                result.Total,
                result.Items.Select(item => _mapper.Map<Channel>(item)));
        }

        private async Task EnsureIndexExistsAsync(CancellationToken cancellationToken = default)
        {
            await _elasticSearchManagementRepository.EnsureIndexExistsAsync(ChannelsIndexName, idx => idx.Map(map => map
                .Properties<ChannelElasticModel>(p => p
                    .Keyword(field => field.Name("channelId"))
                    .Text(field => field.Name("name"))
                    .Text(field => field.Name("url"))
                    .Text(field => field.Name("copyright"))
                    .Text(field => field.Name("imageUrl"))
                    .DateNanos(field => field.Name("createdAt").Format("strict_date_optional_time_nanos"))
                )), cancellationToken);
        }
    }
}
