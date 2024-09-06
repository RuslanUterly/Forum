using AutoMapper;
using Persistance.Dto.Users;
using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles() 
    {
        CreateMap<User, DataUserRequest>().ReverseMap();
        CreateMap<User, ChangeUserRequest>().ReverseMap();
    }
}
