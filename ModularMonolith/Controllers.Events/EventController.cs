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
        await EventRepository.Add(new Event(eventId, payload.EventName, payload.StartDate, payload.EndDate, payload.Venue, payload.Price));
        return Created($"/{Routes.Events}/{eventId}", eventId);
    }

    [HttpPut(Routes.TheEvent)]
    public async Task<ActionResult> UpdateEvent(Guid id, [FromBody] UpdateEventPayload payload)
    {
        ValidateDate(payload);
        await EventRepository.Update(id, payload.EventName, payload.StartDate, payload.EndDate, payload.Price);
        return NoContent();
    }
    
    private static void ValidateDate(EventPayload payload) => ValidateDate(payload.StartDate);

    private static void ValidateDate(UpdateEventPayload payload) => ValidateDate(payload.StartDate);
    
    private static void ValidateDate(DateTimeOffset startDate)
    {
        if (startDate < DateTimeOffset.Now) throw new ValidationException("Event date cannot be in the past");
    }
}