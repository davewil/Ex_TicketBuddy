using MassTransit;

namespace Messaging.InternalConsumers
{
    public class InternalEventConsumerDefinition :
        ConsumerDefinition<InternalEventConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<InternalEventConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}