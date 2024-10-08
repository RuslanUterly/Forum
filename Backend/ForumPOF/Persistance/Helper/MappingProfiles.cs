using AutoMapper;
using Persistance.Dto.Categories;
using Persistance.Dto.Posts;
using Persistance.Dto.Tags;
using Persistance.Dto.Topics;
using Persistance.Dto.Users;
using Persistance.Models;

namespace Persistance.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles() 
    {
        CreateMap<User, DataUserRequest>().ReverseMap();
        CreateMap<User, ChangeUserRequest>().ReverseMap();
        CreateMap<Category, CategoryRequest>().ReverseMap();
        CreateMap<Category, CategoryDetailsRequest>().ReverseMap();
        CreateMap<Topic, TopicDetailsRequest>().ReverseMap();
        CreateMap<Tag, TagDetailsRequest>().ReverseMap();
        CreateMap<Tag, TagRequest>().ReverseMap();
        CreateMap<Post, PostDetailRequest>().ReverseMap();
    }
}
