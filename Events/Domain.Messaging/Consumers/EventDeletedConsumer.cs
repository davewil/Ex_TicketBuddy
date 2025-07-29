using MassTransit;
using EventDeleted = Domain.Messaging.Messages.EventDeleted;

namespace Domain.Messaging.Consumers
{
    public class EventDeletedConsumer : IConsumer<EventDeleted>
    {
        public async Task Consume(ConsumeContext<EventDeleted> context)
        {
            await context.Publish(new Integration.Messaging.Messages.EventDeleted{ Id = context.Message.Id});
        }
    }
}