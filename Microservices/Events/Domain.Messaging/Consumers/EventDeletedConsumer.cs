using MassTransit;
using EventDeleted = Events.Domain.Messaging.Messages.EventDeleted;

namespace Events.Domain.Messaging.Consumers
{
    public class EventDeletedConsumer : IConsumer<EventDeleted>
    {
        public async Task Consume(ConsumeContext<EventDeleted> context)
        {
            await context.Publish(new Events.Integration.Messaging.Outbound.Messages.EventDeleted{ Id = context.Message.Id});
        }
    }
}