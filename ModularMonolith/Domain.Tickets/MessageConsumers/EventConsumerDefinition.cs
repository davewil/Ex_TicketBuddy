using Domain.Tickets.DomainMessageConsumers;
using MassTransit;

namespace Domain.Tickets.MessageConsumers
{
    public class EventConsumerDefinition : ConsumerDefinition<EventConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<EventConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}