using AutoMapper;
using Persistance.Dto.Categories;
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
    }
}
