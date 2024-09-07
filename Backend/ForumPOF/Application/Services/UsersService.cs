using Application.Helper;
using Application.Interfaces.Auth;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class UsersService(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IJwtProvider jwtProvider)
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<bool> Register(string userName, string email, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);

        var user = User.Create(Ulid.NewUlid(), userName, hashedPassword, email, DateTime.Now);

        return await _userRepository.CreateUser(user);
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);

        var result = _passwordHasher.Verify(password, user.Password);

        if (result is false)
            throw new Exception("Ошибка авторизации! Проверьте логин или пароль");

        var token = _jwtProvider.GenerateToken(user);

        return token;
    }

    public async Task<string> Reestablish(string email, string password)
    {

        var user = await _userRepository.GetUserByEmail(email);
        if (user is null)
            throw new Exception("Пользователь не найден");

        var passwordHash = _passwordHasher.Generate(password);

        user = User.Update(user, passwordHash, email, DateTime.Now);
        return await _userRepository!.UpdateUser(user) ? 
            "Пароль успешно изменен" 
            : 
            "Ошибка изменения пароля";
    }

    //////public Task<JwtSecurityToken> Verify(string jwt)
    //////{
    //////    return Task.FromResult(_jwtProvider.VerifyToken(jwt));
    //////}

    public async Task<User> Recieve(string jwt)
    {
        Ulid id = Reciever.RecieveUlid(_jwtProvider, jwt);
        
        return await _userRepository.GetUserById(id);
    }

    public async Task<IEnumerable<User>> RecieveAll()
    {
        return await _userRepository.GetUsers();
    }

    public async Task<bool> Update(string jwt, string email, string password)
    {
        var user = await Recieve(jwt);

        var passwordHash = _passwordHasher.Generate(password);

        user = User.Update(user, passwordHash, email, DateTime.Now);

        return await _userRepository!.UpdateUser(user);    
    }

    public async Task<bool> Delete(string jwt)
    {
        var user = await Recieve(jwt);

        return await _userRepository.DeleteUser(user);
    }
}
