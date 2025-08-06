using Api.Hosting;
using Api.Requests;
using Application;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route(Routes.Event)]
public class EventController(EventService EventService) : ControllerBase
{
    [HttpGet]
    public async Task<IList<Event>> GetEvents()
    {
        return await EventService.GetAll();
    }    
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Event>> GetEvent(Guid id)
    {
        var user = await EventService.Get(id);
        if (user is null) return NotFound();
        return user;
    }    
    
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateEvent([FromBody] EventPayload payload)
    {
        var id = await EventService.Add(payload.Name);
        return Created($"/{Routes.Event}/{id}", id);
    }    
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateEvent(Guid id, [FromBody] EventPayload payload)
    {
        await EventService.Update(id, payload.Name);
        return NoContent();
    }
}