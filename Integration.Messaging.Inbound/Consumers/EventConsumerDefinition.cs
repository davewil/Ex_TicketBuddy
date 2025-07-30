using MassTransit;

namespace Tickets.Integration.Messaging.Inbound.Consumers
{
    public class EventConsumerDefinition : ConsumerDefinition<Domain.Messaging.Consumers.EventConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<Domain.Messaging.Consumers.EventConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}