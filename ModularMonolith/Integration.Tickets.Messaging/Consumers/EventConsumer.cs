using Integration.Events.Messaging;
using MassTransit;
using Persistence.Tickets.Events;
using Event = Domain.Tickets.Entities.Event;

namespace Integration.Tickets.Messaging.Consumers
{
    public class EventConsumer(EventRepository EventRepository) : IConsumer<EventUpserted>
    {
        public async Task Consume(ConsumeContext<EventUpserted> context)
        {
            await EventRepository.Save(new Event(context.Message.Id,context.Message.Name, context.Message.StartDate, context.Message.EndDate, context.Message.Venue));
        }
    }
}