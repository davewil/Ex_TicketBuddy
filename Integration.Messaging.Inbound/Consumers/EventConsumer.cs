using Integration.Messaging.Messages;
using MassTransit;
using Persistence;
using Event = Domain.Entities.Event;

namespace Tickets.Integration.Messaging.Inbound.Consumers
{
    public class EventConsumer(EventRepository EventRepository) : IConsumer<EventUpserted>
    {
        public async Task Consume(ConsumeContext<EventUpserted> context)
        {
            await EventRepository.Save(new Event(context.Message.Id,context.Message.Name));
        }
    }
}