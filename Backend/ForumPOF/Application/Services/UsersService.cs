using Application.DTOs.Users;
using Application.Helper;
using Application.Interfaces.Auth;
using Mapster;
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

    public async Task<IEnumerable<UserDetailsRequest>> RecieveAll()
    {
        var users = await _userRepository.GetUsers();
        return users.Adapt<IEnumerable<UserDetailsRequest>>();
    }

    public async Task<UserDetailsRequest> RecieveUser(string jwt)
    {
        Ulid id = Reciever.UserUlid(_jwtProvider, jwt);
        if (id == default)
            return new UserDetailsRequest();

        var user = await _userRepository.GetUserById(id);
        return user.Adapt<UserDetailsRequest>();
    }

    public async Task<Result> Register(RegisterUserRequest userRequest)
    {
        if (await _userRepository.UserExistByEmail(userRequest.Email))
            return Result.Failure("Пользователь уже зарегистрирован");

        if (await _userRepository.UserExistByUsername(userRequest.UserName))
            return Result.Failure("Имя пользователя занято");

        var hashedPassword = _passwordHasher.Generate(userRequest.Password);

        var user = User.Create(Ulid.NewUlid(), userRequest.UserName, hashedPassword, userRequest.Email, DateTime.Now);

        var isCreated = await _userRepository.CreateUser(user);
        return isCreated ?
            Result.Success("Успех!") :
            Result.Failure("Ошибка");
    }

    public async Task<Result> Login(LoginUserRequest userRequest)
    {
        if (!await _userRepository.UserExistByEmail(userRequest.Email))
            return Result.Failure("Пользователь не найден");

        var user = await _userRepository.GetUserByEmail(userRequest.Email);

        var result = _passwordHasher.Verify(userRequest.Password, user.Password);

        if (result is false)
            return Result.Failure("Ошибка авторизации! Проверьте логин или пароль");

        var token = _jwtProvider.GenerateToken(user);

        return Result.Success(token);
    }

    public async Task<Result> Reestablish(ReestablishUserRequest userRequest)
    {
        if (!await _userRepository.UserExistByEmail(userRequest.Email))
            return Result.Failure("Пользователь не найден");

        var passwordHash = _passwordHasher.Generate(userRequest.Password);

        var user = await _userRepository.GetUserByEmail(userRequest.Email);

        user = User.Reestablish(user, passwordHash, userRequest.Email, DateTime.Now);

        var isUpdated = await _userRepository.UpdateUser(user);

        return isUpdated ?
            Result.Success("Пароль успешно изменен") :
            Result.Failure("Ошибка изменения пароля");
    }

    public async Task<Result> Update(string jwt, UserUpdateRequest userRequest)
    {
        Ulid id = Reciever.UserUlid(_jwtProvider, jwt);
        if (id == default)
            return Result.Failure("Пользователя не существует");

        var passwordHash = _passwordHasher.Generate(userRequest.Password);

        var user = await _userRepository.GetUserById(id);

        user = User.Update(user, userRequest.UserName, passwordHash, userRequest.Email, DateTime.Now);

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
