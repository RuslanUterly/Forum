using Application.DTOs.Categories;
using Application.DTOs.Comments;
using Application.DTOs.Posts;
using Application.DTOs.Tags;
using Application.DTOs.Topics;
using Application.DTOs.Users;
using AutoMapper;
using Microsoft.Extensions.Options;
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
            .AfterMap((src, desc) => desc.Id = Ulid.NewUlid())
            //.AfterMap((src, desc) => desc.Name = src.Name)
            .AfterMap((src, desc) => desc.Created = DateTime.Now);

        CreateMap<CategoryUpdateRequest, Category>();
            //.ForMember(src => src.Id, options => options.Ignore())
            //.ForMember(src => src.Created, options => options.Ignore())
            //.ForMember(src => src.Topics, options => options.Ignore());

        CreateMap<Topic, TopicDetailsRequest>().ReverseMap();

        CreateMap<Tag, TagDetailsRequest>().ReverseMap();
        CreateMap<Tag, TagRequest>().ReverseMap();

        CreateMap<Post, PostDetailRequest>().ReverseMap();

        CreateMap<Comment, CommentDetailsRequest>().ReverseMap();
        CreateMap<CommentCreateRequest, Comment>()
            .AfterMap((src, desc) => desc.Id = Ulid.NewUlid())
            .AfterMap((src, desc, opt) => desc.UserId = (Ulid)opt.Items["userId"])
            .AfterMap((src, desc) => desc.Created = DateTime.Now);
        CreateMap<CommentUpdateRequest, Comment>()
            //.AfterMap((src, desc) => desc.Content = src.Content)
            .AfterMap((src, desc) => desc.Updated = DateTime.Now);
    }
}
