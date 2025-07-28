using MassTransit;
using Messaging.InternalContracts;
using Event = Messaging.ExternalContracts.Event;

namespace Messaging.InternalConsumers
{
    public class InternalEventConsumer : IConsumer<InternalEventUpserted>
    {
        public async Task Consume(ConsumeContext<InternalEventUpserted> context)
        {
           await context.Publish(new Event{ Id = context.Message.Id, Name = context.Message.Name });
        }
    }
}