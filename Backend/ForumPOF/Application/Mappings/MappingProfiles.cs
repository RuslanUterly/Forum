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
            //.ConstructUsing(category => Category.Create(Ulid.NewUlid(), category.Name, DateTime.Now));
            .AfterMap((src, desc) => desc.Id = Ulid.NewUlid())
            .AfterMap((src, desc) => desc.Name = src.Name)
            .AfterMap((src, desc) => desc.Created = DateTime.Now);

        CreateMap<CategoryUpdateRequest, Category>()
            .ForMember(category => category.Id, options => options.Ignore())
            .ForMember(category => category.Created, options => options.Ignore())
            .ForMember(category => category.Topics, options => options.Ignore());

        CreateMap<Topic, TopicDetailsRequest>().ReverseMap();

        CreateMap<Tag, TagDetailsRequest>().ReverseMap();
        CreateMap<Tag, TagRequest>().ReverseMap();

        CreateMap<Post, PostDetailRequest>().ReverseMap();

        CreateMap<Comment, CommentDetailsRequest>().ReverseMap();
        //CreateMap<UpdateCommentRequest, Comment>()
        //    .AfterMap((src, desc) => Comment.Update(desc, src.Content, DateTime.Now));
        CreateMap<UpdateCommentRequest, Comment>()
            .AfterMap((src, desc) => desc.Content = src.Content)
            .AfterMap((src, desc) => desc.Updated = DateTime.Now);
    }
}
