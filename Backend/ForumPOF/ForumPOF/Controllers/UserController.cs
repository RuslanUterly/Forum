﻿using Application.DTOs.Users;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Persistance.Models;

namespace ForumPOF.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController(UsersService usersService) : Controller
{
    private readonly UsersService _usersService = usersService;

    [HttpGet("recieveAllUsers")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _usersService!.RecieveAll();
        
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

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var user = await _usersService.RecieveUser(jwt);

        return Ok(user);
    }


    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest userRequest)
    {
        if (userRequest == null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _usersService.Update(jwt, userRequest);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteUser()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _usersService.Delete(jwt);

        Response.Cookies.Delete("tasty-cookies");

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }
}
