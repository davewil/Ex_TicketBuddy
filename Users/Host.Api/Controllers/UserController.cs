using Api.Hosting;
using Api.Requests;
using Application;
using Microsoft.AspNetCore.Mvc;
using Users.Domain.Entities;

namespace Api.Controllers;

[ApiController]
[Route(Routes.User)]
public class UserController(UserService UserService) : ControllerBase
{
    [HttpGet]
    public async Task<IList<User>> GetUsers()
    {
        return await UserService.GetAll();
    }    
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await UserService.Get(id);
        if (user is null) return NotFound();
        return user;
    }    
    
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] UserPayload payload)
    {
        var id = await UserService.Add(payload.FullName, payload.Email);
        return Created($"/{Routes.User}/{id}", id);
    }    
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateEvent(Guid id, [FromBody] UserPayload payload)
    {
        await UserService.Update(id, payload.FullName, payload.Email);
        return NoContent();
    }
}