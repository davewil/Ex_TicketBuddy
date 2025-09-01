using Controllers.Users.Requests;
using Domain.Users.Entities;
using Microsoft.AspNetCore.Mvc;
using Users.Persistence;

namespace Controllers.Users;

[ApiController]
public class UserController(UserRepository UserRepository) : ControllerBase
{
    [HttpGet(Routes.Users)]
    public async Task<IList<User>> GetUsers()
    {
        return await UserRepository.GetAll();
    }    
    
    [HttpGet(Routes.TheUser)]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await UserRepository.Get(id);
        if (user is null) return NotFound();
        return user;
    }    
    
    [HttpPost(Routes.Users)]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] UserPayload payload)
    {
        var id = Guid.NewGuid();
        var user = new User(id, payload.FullName, payload.Email, payload.UserType);
        await UserRepository.Save(user);
        return Created($"/{Routes.Users}/{id}", id);
    }    
    
    [HttpPut(Routes.TheUser)]
    public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UpdateUserPayload payload)
    {
        var user = await UserRepository.Get(id);
        if (user is null) return NotFound();
        
        user.UpdateName(payload.FullName);
        user.UpdateEmail(payload.Email);
        await UserRepository.Save(user);
        return NoContent();
    }
}