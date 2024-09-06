using Persistance.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Interfaces.Auth;

public interface IJwtProvider
{
    string GenerateToken(User user);
    JwtSecurityToken VerifyToken(string jwt);
}
