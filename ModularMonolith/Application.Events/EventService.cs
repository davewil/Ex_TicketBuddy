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

    public async Task<Guid> Add(EventName name, DateTimeOffset date)
    {
        var id = Guid.NewGuid();
        var user = new Event(id, name, date);
        await repository.Save(user);
        return id;
    }

    public async Task Update(Guid id, EventName name)
    {
        var user = await repository.Get(id);
        if (user is null) return;
        
        user.UpdateName(name);
        await repository.Save(user);
    }
}
