namespace Controllers.Tickets.Requests;

public record TicketPayload(decimal price);
public record TicketPurchasePayload(Guid userId, Guid[] ticketIds);