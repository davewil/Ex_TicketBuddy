using Events.Persistence;
using Integration.Events.Messaging.Outbound;
using Integration.Tickets.Messaging.Outbound;
using MassTransit;

namespace Integration.Events.Messaging.Inbound.Consumers
{
    public class TicketsReleasedConsumer(EventRepository EventRepository) : IConsumer<TicketsReleased>
    {
        public async Task Consume(ConsumeContext<TicketsReleased> context)
        {
            var message = context.Message;

            var @event = await EventRepository.Get(message.EventId);
            if (@event is null) return;

            @event.TicketsHaveBeenReleased();
            await EventRepository.Save(@event);
        }
    }
}