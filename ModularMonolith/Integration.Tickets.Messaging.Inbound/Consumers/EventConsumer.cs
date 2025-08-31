using Integration.Events.Messaging.Outbound;
using MassTransit;
using Persistence.Tickets;
using Event = Domain.Tickets.Entities.Event;

namespace Integration.Tickets.Messaging.Inbound.Consumers
{
    public class EventConsumer(EventRepository EventRepository) : IConsumer<EventUpserted>
    {
        public async Task Consume(ConsumeContext<EventUpserted> context)
        {
            await EventRepository.Save(new Event(context.Message.Id,context.Message.EventName, context.Message.StartDate, context.Message.EndDate, context.Message.Venue));
        }
    }
}