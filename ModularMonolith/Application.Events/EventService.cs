using Domain.Events.Entities;
using Domain.Events.Primitives;
using Events.Persistence;

namespace Application.Events;

public class EventService(EventRepository repository)
{
    public async Task<Event?> Get(Guid id)
    {
        return await repository.Get(id);
    }

    public async Task<IList<Event>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<Guid> Add(Name name, DateTimeOffset date)
    {
        var id = Guid.NewGuid();
        var @event = new Event(id, name, date);
        await repository.Save(@event);
        return id;
    }

    public async Task Update(Guid id, Name name, DateTimeOffset date)
    {
        var @event = await repository.Get(id);
        if (@event is null) return;
        
        @event.UpdateName(name);
        @event.UpdateDate(date);
        await repository.Save(@event);
    }
}
