using MassTransit;
using Persistence.Tickets.Messages;

namespace Persistence.Tickets.Consumers
{
    public class EventConsumer(TicketRepository ticketRepository, EventRepository eventRepository) : IConsumer<EventUpserted>
    {
        public async Task Consume(ConsumeContext<EventUpserted> context)
        {
           if (await ticketRepository.AreTicketsReleasedForEvent(context.Message.Id))
           {
               await ticketRepository.UpdateTicketPricesForEvent(context.Message.Id, context.Message.Price);
               return;
           }
           var venue = await eventRepository.GetVenue(context.Message.Venue);
           await ticketRepository.ReleaseTicketsForEvent(context.Message.Id, venue.Capacity, context.Message.Price);
        }
    }
}