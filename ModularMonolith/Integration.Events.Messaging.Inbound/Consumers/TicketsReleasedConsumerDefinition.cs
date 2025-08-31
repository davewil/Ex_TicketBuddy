using MassTransit;

namespace Integration.Events.Messaging.Inbound.Consumers
{
    public class TicketsReleasedConsumerDefinition : ConsumerDefinition<TicketsReleasedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TicketsReleasedConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}