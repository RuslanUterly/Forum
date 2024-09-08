using Application.Interfaces.Auth;
using Application.Services;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper;

public static class Reciever
{
    public static Ulid UserUlid(IJwtProvider jwtProvider, string jwt)
    {
        var token= jwtProvider.VerifyToken(jwt);

        return Ulid.Parse(token.Payload["userId"].ToString());
    }

    //public static Ulid CategoryUlid(string categoryName)
    //{
        
    //}
}
