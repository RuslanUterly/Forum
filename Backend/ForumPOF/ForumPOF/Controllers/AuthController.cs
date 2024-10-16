using Application.DTOs.Users;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(UsersService usersService) : Controller
{
    private readonly UsersService _usersService = usersService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _usersService.Register(request);

        if (!result.IsSuccess)
            return BadRequest(result.Message);
        
        return Ok(result.Message);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = await usersService.Login(request);

        if (!token.IsSuccess)
            return BadRequest(token.Message);

        Response.Cookies.Append("tasty-cookies", token.Message);

        return Ok(token.Message);
    }

    [HttpPost("reestablish")]
    public async Task<IActionResult> Reestablish([FromBody] ReestablishUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var isReestablished = await usersService.Reestablish(request);

        if (!isReestablished.IsSuccess)
            return BadRequest(isReestablished.Message);

        return Ok(isReestablished.Message);
    }


    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("tasty-cookies");
        return Ok("Successfull");
    }
}
