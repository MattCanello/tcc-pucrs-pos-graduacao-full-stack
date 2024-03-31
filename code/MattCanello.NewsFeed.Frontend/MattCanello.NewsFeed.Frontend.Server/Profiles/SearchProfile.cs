using AutoMapper;
using MattCanello.NewsFeed.Frontend.Server.Models;
using MattCanello.NewsFeed.Frontend.Server.Models.Search;

namespace MattCanello.NewsFeed.Frontend.Server.Profiles
{
    public sealed class SearchProfile : Profile
    {
        public SearchProfile() {

            CreateMap<SearchAuthor, Author>();

            CreateMap<SearchThumbnail, Thumbnail>()
                .ForMember(thumb => thumb.ImageUrl, o => o.MapFrom(searchThumb => searchThumb.Url))
                .ForMember(thumb => thumb.Caption, o => o.Ignore());
        }
    }
}
