using AutoMapper;
using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Interfaces;
using MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Models;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Factories
{
    public sealed class ElasticModelFactory : IElasticModelFactory
    {
        private readonly IMapper _mapper;

        public ElasticModelFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Entry CreateElasticModel(IndexEntryCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            if (command.Entry is null)
                throw new ArgumentException("The command must include an entry.", nameof(command));

            var elasticModel = _mapper.Map<ElasticSearch.Models.Entry>(command.Entry);

            elasticModel.FeedId = command.FeedId;

            return elasticModel;
        }
    }
}
