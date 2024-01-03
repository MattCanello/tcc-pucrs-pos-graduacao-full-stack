using AutoMapper;
using MattCanello.NewsFeed.RssReader.Events;
using MattCanello.NewsFeed.RssReader.Models;

namespace MattCanello.NewsFeed.RssReader.Profiles
{
    public class FeedProfile : Profile
    {
        public FeedProfile()
        {
            CreateMap<CreateFeedMessage, Feed>();
        }
    }
}
