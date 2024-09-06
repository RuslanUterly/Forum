using Application.Interfaces.Auth;
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
public class UserControllers(IUserRepository userRepository, IMapper mapper, UsersService usersService) : Controller
{
    //private readonly IUserRepository? _userRepository = userRepository;
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
            string jwt = Request.Cookies["tasty-cookies"]!;

            var user = _mapper!.Map<DataUserRequest>(await _usersService.Recieve(jwt));

            return Ok(user);
        }
        catch (Exception _)
        {
            return Unauthorized();
        }
    }


    [HttpPut("updateUser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateUser([FromBody] LoginUserRequest changeUser)
    {
        if (changeUser == null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            string jwt = Request.Cookies["tasty-cookies"]!;

            if (!await _usersService.Update(jwt, changeUser.Email, changeUser.Password))
            {
                ModelState.AddModelError("", "Smth went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfull");
        }
        catch (Exception _)
        {
            return Unauthorized();
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
            string jwt = Request.Cookies["tasty-cookies"]!;

            if(!await _usersService.Delete(jwt))
            {
                ModelState.AddModelError("", "Smth went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfull");
        }
        catch (Exception _)
        {
            return Unauthorized();
        }
    }
}
