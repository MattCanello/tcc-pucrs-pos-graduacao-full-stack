using AutoMapper;
using MattCanello.NewsFeed.Cross.Abstractions.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Factories
{
    public sealed class ElasticModelFactory : IElasticModelFactory
    {
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ElasticModelFactory(IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        public Entry CreateElasticModel(IndexEntryCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            if (command.Entry is null)
                throw new ArgumentException("The command must include an entry.", nameof(command));

            var elasticModel = _mapper.Map<Entry>(command.Entry);

            elasticModel.FeedId = command.FeedId;
            elasticModel.ChannelId = command.ChannelId;
            elasticModel.IndexDate = _dateTimeProvider.GetUtcNow();

            return elasticModel;
        }
    }
}
