using Application.DTOs.Users;
using Application.Services;
using ForumPOF.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.Enums;
using System.Security.Claims;

namespace ForumPOF.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController(UsersService usersService) : Controller
{
    private readonly UsersService _usersService = usersService;

    [HttpGet("all")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDetailsRequest>))]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _usersService!.ReceiveAll();
        
        return Ok(users);
    }

    [AuthorizeByRole]
    [HttpGet("user")]
    [ProducesResponseType(200, Type = typeof(UserDetailsRequest))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUser()
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);

        var user = await _usersService.ReceiveUser(userId);

        return Ok(user);
    }

    [AuthorizeByRole]
    [SkipLogging]
    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest userRequest)
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);
        var userRole = Enum.Parse<UserRole>(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);

        var result = await _usersService.Update(userId, userRole, userRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [AuthorizeByRole]
    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteUser()
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);
        var userRole = Enum.Parse<UserRole>(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);

        var result = await _usersService.Delete(userId, userRole);

        Response.Cookies.Delete("tasty-cookies");

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
