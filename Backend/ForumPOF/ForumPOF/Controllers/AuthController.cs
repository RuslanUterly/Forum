using Application.Interfaces.Auth;
using Application.Services;
using AutoMapper;
using ForumPOF.Contracts.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistance.Dto.Users;
using Persistance.Models;
using System.Diagnostics.Eventing.Reader;

namespace ForumPOF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(UsersService usersService) : Controller
{
    private readonly UsersService _usersService = usersService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _usersService.Register(request.UserName, request.Email, request.Password))
            return BadRequest("User is register");
        
        return Ok("Successfull");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = await usersService.Login(request.Email, request.Password);

        Response.Cookies.Append("tasty-cookies", token);

        return Ok(token);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("tasty-cookies");
        return Ok("Successfull");
    }
    
}
