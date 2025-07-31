using Events.Integration.Messaging.Outbound.Messages;
using MassTransit;
using Persistence;
using Persistence.Events;

namespace Tickets.Integration.Messaging.Inbound.Consumers
{
    public class EventDeletedConsumer(EventRepository repository) : IConsumer<EventDeleted>
    {
        public async Task Consume(ConsumeContext<EventDeleted> context)
        {
            await repository.Remove(context.Message.Id);
        }
    }
}