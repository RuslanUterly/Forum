using System.IdentityModel.Tokens.Jwt;

namespace Application.Interfaces.Auth;

public interface IUsersCrudService
{
    Task<JwtSecurityToken> Verify(string jwt);
    Task<bool> Change(string userId, string email, string password);
}
