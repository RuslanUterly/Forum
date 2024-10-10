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

    public async Task<Result> Register(string userName, string email, string password)
    {
        if (await _userRepository.UserExistByEmail(email))
            return Result.Failure("Пользователь уже зарегистрирован");

        if (await _userRepository.UserExistByUsername(email))
            return Result.Failure("Имя пользователя занято");

        var hashedPassword = _passwordHasher.Generate(password);

        var user = User.Create(Ulid.NewUlid(), userName, hashedPassword, email, DateTime.Now);

        var isCreated = await _userRepository.CreateUser(user);
        return isCreated ?
            Result.Success("Успех!") :
            Result.Failure("Ошибка");
    }

    public async Task<Result> Login(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);

        var result = _passwordHasher.Verify(password, user.Password);

        if (result is false)
            return Result.Failure("Ошибка авторизации! Проверьте логин или пароль");

        var token = _jwtProvider.GenerateToken(user);

        return Result.Success(token);
    }

    public async Task<Result> Reestablish(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user is null)
            return Result.Failure("Пользователь не найден");

        var passwordHash = _passwordHasher.Generate(password);

        user = User.Update(user, passwordHash, email, DateTime.Now);

        var isUpdated = await _userRepository.UpdateUser(user);
        return isUpdated ?
            Result.Success("Пароль успешно изменен") :
            Result.Failure("Ошибка изменения пароля");
    }

    public async Task<IEnumerable<User>> RecieveAll()
    {
        return await _userRepository.GetUsers();
    }

    public async Task<Result> Update(string jwt, string email, string password)
    {
        Ulid id = Reciever.UserUlid(_jwtProvider, jwt);
        if (id == default)
            return Result.Failure("Пользователя не существует");

        var user = await _userRepository.GetUserById(id);

        var passwordHash = _passwordHasher.Generate(password);

        user = User.Update(user, passwordHash, email, DateTime.Now);

        var isUpdated = await _userRepository!.UpdateUser(user);

        return isUpdated ?
            Result.Success("Пользователь изменен") :
            Result.Failure("Произошла ошибка");    
    }

    public async Task<Result> Delete(string jwt)
    {
        Ulid id = Reciever.UserUlid(_jwtProvider, jwt);
        if (id == default)
            return Result.Failure("Пользователя не существует");

        var user = await _userRepository.GetUserById(id);

        var isRemoved = await _userRepository.DeleteUser(user);

        return isRemoved ? 
            Result.Success("Пользователь удален") :
            Result.Failure("Произошла ошибка");
    }
}
