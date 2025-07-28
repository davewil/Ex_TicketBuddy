using MassTransit;
using Messaging.ExternalContracts;
using Messaging.InternalContracts;

namespace Messaging.InternalConsumers
{
    public class InternalEventDeletedConsumer : IConsumer<InternalEventDeleted>
    {
        public async Task Consume(ConsumeContext<InternalEventDeleted> context)
        {
           await context.Publish(new EventDeleted{ Id = context.Message.Id});
        }
    }
}