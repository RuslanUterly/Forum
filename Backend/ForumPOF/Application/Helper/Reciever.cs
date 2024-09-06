using Application.Interfaces.Auth;
using Application.Services;
using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper;

public static class Reciever
{
    public static Ulid RecieveUlid(IJwtProvider jwtProvider, string jwt)
    {
        var token= jwtProvider.VerifyToken(jwt);

        return Ulid.Parse(token.Payload["userId"].ToString());
    }
}
