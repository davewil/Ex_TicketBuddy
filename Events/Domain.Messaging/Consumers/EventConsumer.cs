using Domain.Messaging.Messages;
using MassTransit;

namespace Domain.Messaging.Consumers
{
    public class EventConsumer : IConsumer<EventUpserted>
    {
        public async Task Consume(ConsumeContext<EventUpserted> context)
        {
           await context.Publish(new Integration.Messaging.Messages.EventUpserted{ Id = context.Message.Id, Name = context.Message.Name });
        }
    }
}