using MassTransit;
using Tickets.Integration.Messaging.Inbound.Consumers;

namespace Domain.Messaging.Consumers
{
    public class InternalEventDeletedConsumerDefinition : ConsumerDefinition<EventDeletedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<EventDeletedConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}