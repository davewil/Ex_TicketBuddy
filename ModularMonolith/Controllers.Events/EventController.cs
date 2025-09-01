using System.ComponentModel.DataAnnotations;
using Controllers.Events.Requests;
using Domain.Events.Entities;
using Events.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Events;

[ApiController]
public class EventController(EventRepository EventRepository) : ControllerBase
{
    [HttpGet(Routes.Events)]
    public async Task<IList<Event>> GetEvents()
    {
        return await EventRepository.GetAll();
    }    
    
    [HttpGet(Routes.TheEvent)]
    public async Task<ActionResult<Event>> GetEvent(Guid id)
    {
        var user = await EventRepository.Get(id);
        if (user is null) return NotFound();
        return user;
    }    
    
    [HttpPost(Routes.Events)]
    public async Task<ActionResult<Guid>> CreateEvent([FromBody] EventPayload payload)
    {
        ValidateDate(payload);
        var eventId = Guid.NewGuid();
        await EventRepository.Save(new Event(eventId, payload.EventName, payload.StartDate, payload.EndDate, payload.Venue, payload.Price));
        return Created($"/{Routes.Events}/{eventId}", eventId);
    }

    [HttpPut(Routes.TheEvent)]
    public async Task<ActionResult> UpdateEvent(Guid id, [FromBody] EventPayload payload)
    {
        ValidateDate(payload);
        var @event = await EventRepository.Get(id);
        if (@event is null) return NotFound();
        
        @event.UpdateName(payload.EventName);
        @event.UpdateDates(payload.StartDate, payload.EndDate);
        @event.UpdateVenue(payload.Venue);
        @event.UpdatePrice(payload.Price);
        await EventRepository.Save(@event);
        return NoContent();
    }
    
    private static void ValidateDate(EventPayload payload)
    {
        if (payload.StartDate < DateTimeOffset.Now) throw new ValidationException("Event date cannot be in the past");
    }
}