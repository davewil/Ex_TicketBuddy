using Application;
using Controllers.Users.Requests;
using Domain.Users.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Users;

[ApiController]
public class UserController(UserService UserService) : ControllerBase
{
    [HttpGet(Routes.Users)]
    public async Task<IList<User>> GetUsers()
    {
        return await UserService.GetAll();
    }    
    
    [HttpGet(Routes.TheUser)]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await UserService.Get(id);
        if (user is null) return NotFound();
        return user;
    }    
    
    [HttpPost(Routes.Users)]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] UserPayload payload)
    {
        var id = await UserService.Add(payload.FullName, payload.Email, payload.UserType);
        return Created($"/{Routes.Users}/{id}", id);
    }    
    
    [HttpPut(Routes.TheUser)]
    public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UpdateUserPayload payload)
    {
        await UserService.Update(id, payload.FullName, payload.Email);
        return NoContent();
    }
}