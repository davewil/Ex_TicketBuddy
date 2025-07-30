using MassTransit;

namespace Tickets.Integration.Messaging.Inbound.Consumers
{
    public class EventConsumerDefinition : ConsumerDefinition<Events.Domain.Messaging.Consumers.EventConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<Events.Domain.Messaging.Consumers.EventConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}