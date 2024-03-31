using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Models;
using MattCanello.NewsFeed.Frontend.Server.Models.Search;

namespace MattCanello.NewsFeed.Frontend.Server.Profiles
{
    public sealed class SearchProfile : Profile
    {
        public SearchProfile() {

            CreateMap<SearchAuthor, Author>()
                .ForMember(author => author.Name, o => o.MapFrom(searchAuthor => searchAuthor.Name))
                .ForMember(author => author.Email, o => o.MapFrom(searchAuthor => searchAuthor.Email));

            CreateMap<SearchThumbnail, Thumbnail>()
                .ForMember(thumb => thumb.ImageUrl, o => o.MapFrom(searchThumb => searchThumb.Url))
                .ForMember(thumb => thumb.Caption, o => o.Ignore())
                .ForMember(thumb => thumb.Credit, o => o.MapFrom(searchThumb => searchThumb.Credit));
        }
    }
}
