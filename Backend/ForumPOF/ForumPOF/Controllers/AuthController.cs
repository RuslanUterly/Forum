using Application.DTOs.Users;
using Application.Services;
using ForumPOF.Attributes;
using ForumPOF.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("[controller]")]
[ApiController]
[SkipLogging]
public class AuthController(UsersService usersService) : ControllerBase
{
    private readonly UsersService _usersService = usersService;

    //[ServiceFilter(typeof(RegisterFilter))]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest userRequest)
    {
        var result = await _usersService.Register(userRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(Register), new { id = result.Data }, result.Data);
    }

    //[ServiceFilter(typeof(UserExistFilter))]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest userRequest)
    {
        var token = await usersService.Login(userRequest);

        if (!token)
            return StatusCode(token.StatusCode, token.Error);

        Response.Cookies.Append("tasty-cookies", token.Data);

        return Ok(token.Data);
    }

    //[ServiceFilter(typeof(UserExistFilter))]
    [HttpPost("reestablish")]
    public async Task<IActionResult> Reestablish([FromBody] ReestablishUserRequest userRequest)
    {
        var isReestablished = await usersService.Reestablish(userRequest);

        if (!isReestablished)
            return StatusCode(isReestablished.StatusCode, isReestablished.Error);

        return Ok();
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("tasty-cookies");
        return Ok();
    }
}
