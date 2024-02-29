using AutoMapper;

namespace MattCanello.NewsFeed.SearchApi.Infrastructure.ElasticSearch.Profiles
{
    public class ElasticSearchModelProfile : Profile
    {
        public ElasticSearchModelProfile()
        {
            CreateMap<Domain.Models.Category, ElasticSearch.Models.Entry.Category>()
                .ForMember(dest => dest.Name, o => o.MapFrom(source => source.CategoryName))
                .ReverseMap();

            CreateMap<Domain.Models.Author, ElasticSearch.Models.Entry.Author>()
                .ReverseMap();

            CreateMap<Domain.Models.Thumbnail, ElasticSearch.Models.Entry.Thumbanil>()
                .ReverseMap();

            CreateMap<Domain.Models.Entry, ElasticSearch.Models.Entry>()
                .ForMember(dest => dest.EntryId, o => o.MapFrom(source => source.Id))
                .ForMember(dest => dest.FeedId, o => o.Ignore())
                .ReverseMap();
        }
    }
}
