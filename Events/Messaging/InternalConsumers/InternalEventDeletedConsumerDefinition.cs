using MassTransit;

namespace Messaging.InternalConsumers
{
    public class InternalEventDeletedConsumerDefinition :
        ConsumerDefinition<InternalEventDeletedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<InternalEventDeletedConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}