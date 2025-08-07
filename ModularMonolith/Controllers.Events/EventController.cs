using System.ComponentModel.DataAnnotations;
using Application.Events;
using Controllers.Events.Requests;
using Domain.Events.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Events;

[ApiController]
public class EventController(EventService EventService) : ControllerBase
{
    [HttpGet(Routes.Events)]
    public async Task<IList<Event>> GetEvents()
    {
        return await EventService.GetAll();
    }    
    
    [HttpGet(Routes.TheEvent)]
    public async Task<ActionResult<Event>> GetEvent(Guid id)
    {
        var user = await EventService.Get(id);
        if (user is null) return NotFound();
        return user;
    }    
    
    [HttpPost(Routes.Events)]
    public async Task<ActionResult<Guid>> CreateEvent([FromBody] EventPayload payload)
    {
        ValidateDate(payload);
        var id = await EventService.Add(payload.Name, payload.Date, payload.Venue);
        return Created($"/{Routes.Events}/{id}", id);
    }

    [HttpPut(Routes.TheEvent)]
    public async Task<ActionResult> UpdateEvent(Guid id, [FromBody] EventPayload payload)
    {
        ValidateDate(payload);
        await EventService.Update(id, payload.Name, payload.Date, payload.Venue);
        return NoContent();
    }
    
    private static void ValidateDate(EventPayload payload)
    {
        if (payload.Date < DateTimeOffset.Now) throw new ValidationException("Event date cannot be in the past");
    }
}