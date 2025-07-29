using Domain.Messaging.Messages;
using MassTransit;
using EventDeleted = Domain.Messaging.Messages.EventDeleted;

namespace Domain.Messaging.Consumers
{
    public class EventConsumer : IConsumer<EventUpserted>
    {
        public async Task Consume(ConsumeContext<EventUpserted> context)
        {
           await context.Publish(new Integration.Messaging.Messages.EventUpserted{ Id = context.Message.Id, Name = context.Message.Name });
        }
    }
    
    public class EventDeletedConsumer : IConsumer<EventDeleted>
    {
        public async Task Consume(ConsumeContext<EventDeleted> context)
        {
            await context.Publish(new Integration.Messaging.Messages.EventDeleted{ Id = context.Message.Id});
        }
    }
}