using Application.DTOs.Users;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [Authorize]
    [HttpGet("user")]
    [ProducesResponseType(200, Type = typeof(UserDetailsRequest))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUser()
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);

        var user = await _usersService.ReceiveUser(userId);

        return Ok(user);
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest userRequest)
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);

        var result = await _usersService.Update(userId, userRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [Authorize]
    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteUser()
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);

        var result = await _usersService.Delete(userId);

        Response.Cookies.Delete("tasty-cookies");

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
