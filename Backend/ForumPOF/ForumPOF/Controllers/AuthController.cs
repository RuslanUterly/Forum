using Application.DTOs.Users;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(UsersService usersService) : ControllerBase
{
    private readonly UsersService _usersService = usersService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var result = await _usersService.Register(request);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(Register), new { id = result.Data }, result.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var token = await usersService.Login(request);

        if (!token)
            return StatusCode(token.StatusCode, token.Error);

        Response.Cookies.Append("tasty-cookies", token.Data);

        return Ok(token.Data);
    }

    [HttpPost("reestablish")]
    public async Task<IActionResult> Reestablish([FromBody] ReestablishUserRequest request)
    {
        var isReestablished = await usersService.Reestablish(request);

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
