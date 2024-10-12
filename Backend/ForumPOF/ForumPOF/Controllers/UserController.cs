using Application.DTOs.Users;
using Application.Interfaces.Auth;
using Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Persistance.Models;
using Persistance.Repository.Interfaces;

namespace ForumPOF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IMapper mapper, UsersService usersService) : Controller
{
    private readonly IMapper? _mapper = mapper;
    private readonly UsersService _usersService = usersService;

    [HttpGet("recieveAllUsers")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
    public async Task<IActionResult> GetUsers()
    {
        var users = _mapper!.Map<List<DataUserRequest>>(await _usersService!.RecieveAll());
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(users);
    }

    [HttpGet("recieveUser")]
    [ProducesResponseType(200, Type = typeof(User))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUser()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var user = _mapper!.Map<DataUserRequest>(await _usersService.RecieveUser(jwt));

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("updateUser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateUser([FromBody] ChangeUserRequest userRequest)
    {
        if (userRequest == null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _usersService.Update(jwt, userRequest.Email, userRequest.Password);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteUser()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _usersService.Delete(jwt);

            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
