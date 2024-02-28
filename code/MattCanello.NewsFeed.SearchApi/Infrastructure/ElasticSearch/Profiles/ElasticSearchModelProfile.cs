using AutoMapper;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Profiles
{
    public class ElasticSearchModelProfile : Profile
    {
        public ElasticSearchModelProfile()
        {
            CreateMap<Domain.Models.Category, ElasticSearch.Models.EntryElasticSearchModel.Category>()
                .ForMember(dest => dest.Name, o => o.MapFrom(source => source.CategoryName))
                .ReverseMap();

            CreateMap<Domain.Models.Author, ElasticSearch.Models.EntryElasticSearchModel.Author>()
                .ReverseMap();

            CreateMap<Domain.Models.Thumbnail, ElasticSearch.Models.EntryElasticSearchModel.Thumbanil>()
                .ReverseMap();

            CreateMap<Domain.Models.Entry, ElasticSearch.Models.EntryElasticSearchModel>()
                .ForMember(dest => dest.EntryId, o => o.MapFrom(source => source.Id))
                .ReverseMap();
        }
    }
}
