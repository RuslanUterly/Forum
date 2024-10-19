using Application.DTOs.Users;
using Application.Services;
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

    [HttpGet("user")]
    [ProducesResponseType(200, Type = typeof(UserDetailsRequest))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUser()
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var user = await _usersService.ReceiveUser(jwt);

        return Ok(user);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest userRequest)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _usersService.Update(jwt, userRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteUser()
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _usersService.Delete(jwt);

        Response.Cookies.Delete("tasty-cookies");

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
