﻿using Application.Interfaces.Auth;
using Application.Services;
using AutoMapper;
using ForumPOF.Contracts.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Persistance.Dto.Users;
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

            var user = _mapper!.Map<DataUserRequest>(await _usersService.Recieve(jwt));

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

            if (!await _usersService.Update(jwt, userRequest.Email, userRequest.Password))
            {
                ModelState.AddModelError("", "Smth went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfull");
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

            if (!await _usersService.Delete(jwt))
            {
                ModelState.AddModelError("", "Smth went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfull");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
