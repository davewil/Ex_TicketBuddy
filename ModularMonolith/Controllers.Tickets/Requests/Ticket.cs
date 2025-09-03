namespace Controllers.Tickets.Requests;

public record TicketPurchasePayload(Guid userId, Guid[] ticketIds);