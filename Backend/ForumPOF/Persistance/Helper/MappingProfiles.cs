using AutoMapper;
using Microsoft.Extensions.Options;
using Persistance.Dto.Categories;
using Persistance.Dto.Comments;
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

        CreateMap<Category, CategoryDetailsRequest>().ReverseMap();

        CreateMap<CategoryCreateRequest, Category>()
            .ConstructUsing(category => Category.Create(Ulid.NewUlid(), category.Name, DateTime.Now));

        CreateMap<CategoryUpdateRequest, Category>()
            .ForMember(category => category.Id, options => options.Ignore())
            .ForMember(category => category.Created, options => options.Ignore())
            .ForMember(category => category.Topics, options => options.Ignore());

        CreateMap<Topic, TopicDetailsRequest>().ReverseMap();

        CreateMap<Tag, TagDetailsRequest>().ReverseMap();
        CreateMap<Tag, TagRequest>().ReverseMap();

        CreateMap<Post, PostDetailRequest>().ReverseMap();

        CreateMap<Comment, CommentDetailsRequest>().ReverseMap();
    }
}
