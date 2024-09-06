using System.IdentityModel.Tokens.Jwt;

namespace Application.Interfaces.Auth;

public interface IUsersService
{
    Task Register(string userName, string email, string password);
    Task<string> Login(string email, string password);
    Task<JwtSecurityToken> Verify(string jwt);
}
