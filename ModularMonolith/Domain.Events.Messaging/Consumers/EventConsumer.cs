using Domain.Events.Messaging.Messages;
using MassTransit;

namespace Domain.Events.Messaging.Consumers
{
    public class EventConsumer : IConsumer<EventUpserted>
    {
        public async Task Consume(ConsumeContext<EventUpserted> context)
        {
           await context.Publish(new Integration.Events.Messaging.Outbound.EventUpserted
           {
               Id = context.Message.Id, 
               EventName = context.Message.Name,
               StartDate = context.Message.StartDate,
               EndDate = context.Message.EndDate,
               Venue = context.Message.Venue
           });
        }
    }
}