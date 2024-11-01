using Application.DTOs.Users;
using Application.Helper;
using Application.Interfaces.Auth;
using Mapster;
using Microsoft.AspNetCore.Http;
using Persistance.Models;
using Persistance.Repository.Interfaces;

namespace Application.Services;

public class UsersService(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IJwtProvider jwtProvider)
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<IEnumerable<UserDetailsRequest>> ReceiveAll()
    {
        var users = await _userRepository.GetUsers();
        return users.Adapt<IEnumerable<UserDetailsRequest>>();
    }

    public async Task<UserDetailsRequest> ReceiveUser(Ulid userId)
    {
        //Ulid id = Reciever.UserUlid(_jwtProvider, jwt);
        //if (id == default)
        //    return new UserDetailsRequest();

        var user = await _userRepository.GetUserById(userId);
        return user.Adapt<UserDetailsRequest>();
    }

    public async Task<Result<Ulid>> Register(RegisterUserRequest userRequest)
    {
        //if (await _userRepository.UserExistByEmail(userRequest.Email))
        //    return Result<Ulid>.BadRequest("Пользователь уже зарегистрирован");

        //if (await _userRepository.UserExistByUsername(userRequest.UserName))
        //    return Result<Ulid>.BadRequest("Имя пользователя занято");

        var hashedPassword = _passwordHasher.Generate(userRequest.Password);

        var user = User.Create(Ulid.NewUlid(), userRequest.UserName, hashedPassword, userRequest.Email, DateTime.Now);

        var isCreated = await _userRepository.CreateUser(user);
        return isCreated ?
            Result<Ulid>.Created(user.Id) :
            Result<Ulid>.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result<string>> Login(LoginUserRequest userRequest)
    {
        //if (!await _userRepository.UserExistByEmail(userRequest.Email))
        //    return Result<string>.NotFound("Пользователь не найден");

        var user = await _userRepository.GetUserByEmail(userRequest.Email);

        var result = _passwordHasher.Verify(userRequest.Password, user.Password);

        if (result is false)
            return Result<string>.BadRequest("Ошибка авторизации! Проверьте логин или пароль");

        var token = _jwtProvider.GenerateToken(user);

        return Result<string>.Ok(token);
    }

    public async Task<Result> Reestablish(ReestablishUserRequest userRequest)
    {
        //if (!await _userRepository.UserExistByEmail(userRequest.Email))
        //    return Result.NotFound("Пользователь не найден");

        var passwordHash = _passwordHasher.Generate(userRequest.Password);

        var user = await _userRepository.GetUserByEmail(userRequest.Email);

        user = User.Reestablish(user, passwordHash, userRequest.Email, DateTime.Now);

        var isUpdated = await _userRepository.UpdateUser(user);

        return isUpdated ?
            Result.Ok() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Update(Ulid userId, UserUpdateRequest userRequest)
    {
        var passwordHash = _passwordHasher.Generate(userRequest.Password);

        var user = await _userRepository.GetUserById(userId);
        if (user.Id != userId)
            return Result.Fail(403, "У вас нет доступа к данной операции");

        user = User.Update(user, userRequest.UserName, passwordHash, userRequest.Email, DateTime.Now);

        var isUpdated = await _userRepository!.UpdateUser(user);

        return isUpdated ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Delete(Ulid userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user.Id != userId)
            return Result.Fail(403, "У вас нет доступа к данной операции");

        var isRemoved = await _userRepository.DeleteUser(user);

        return isRemoved ? 
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }
}
