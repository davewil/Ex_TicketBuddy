using MassTransit;
using Event = Messaging.Contracts.Event;

namespace Messaging.Consumers
{
    public class EventConsumer : IConsumer<Event>
    {
        public Task Consume(ConsumeContext<Event> context)
        {
            Console.WriteLine($"hit: {context.Message.Name}");
            return Task.CompletedTask;
        }
    }
}